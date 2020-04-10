import os, sys, json, re
import numpy as np
import pandas as pd

from sklearn.feature_extraction.text import CountVectorizer

from collections import defaultdict

mentor_dict = {
    '0': 'Bia',
    '1': 'Arthur',
    '2': 'Ceci',
    '3': 'Pedro',
    '4': 'Ivan'
}

mission_dict = {
    'M004': 'baleia',
    'M009': 'paleo',
    'M010': 'vegetacao'
}

def get_dialogue_json(vide_location):
    with open(vide_location) as f:
        return json.load(f)

def get_vide_dialogues(dialogue_json):
    dialogues_texts = list()
    dialogues_ids = list()
    for key, value in dialogue_json.items():
        match = re.match("pd_([\d]+)_com_[\d]+text", key)
        if match: #If there were a match
            dialogues_ids.append(match.group(1))
            dialogues_texts.append(value)
    return dialogues_ids, dialogues_texts

def colnum_string(n):
    string = ""
    while n > 0:
        n, remainder = divmod(n - 1, 26)
        string = chr(65 + remainder) + string
    return string

def get_table_dict(csv_location):
    #table_data = pd.read_csv(csv_location, encoding="latin", header=None).fillna("").values
    table_data = pd.read_excel(csv_location, encoding="latin", header=None).fillna("").values
    
    #[Linha, Coluna]
    table_dict = dict()

    for i in range(table_data.shape[0]):
        for j in range(table_data.shape[1]):
            if table_data[i,j]: #If there is data here
                data_key = colnum_string(j+1) + str(i+1)
                table_dict[data_key] = table_data[i,j]
    return table_dict

def get_dialogue_location(table_dict, dialogues):
    vectorizer = CountVectorizer(strip_accents='ascii', binary=True)
    table_values_vect = vectorizer.fit_transform(table_dict.values()) #Convert table texts to binary vectors
    dialogue_vect = vectorizer.transform(dialogues) #Convert dialogues do binary vectors
    scores = dialogue_vect.dot(table_values_vect.T) #Take dot product between to get the similarities
    keys_indexes = scores.toarray().argmax(axis=1) #Get the indexes of the text keys
    return np.array(list(table_dict.keys()))[keys_indexes] #Return key of each index

def add_dialogue_extravar(dialogue_json, dialogue_id, extravars):
    new_dialogue_json = dialogue_json.copy()
    for k, v in extravars.items():
        var_pos = 0
        #If the var key exists and the key is different, keeps searching
        while 'pd_varKey_{}_{}'.format(dialogue_id, var_pos) in new_dialogue_json: #and new_dialogue_json['pd_varKey_{}_{}'.format(dialogue_id, var_pos)] != k:
            var_pos += 1
        new_dialogue_json['pd_varKey_{}_{}'.format(dialogue_id, var_pos)] = k
        new_dialogue_json['pd_var_{}_{}'.format(dialogue_id, var_pos)] = v
        new_dialogue_json['pd_vars{}'.format(dialogue_id)] += 1
        new_dialogue_json['pd_expand_{}'.format(dialogue_id)] = True
    return new_dialogue_json

def populate_dialogue_with_videos(dialog_path):

    filename = os.path.split(dialog_path)[-1]
    match = re.match("(M[\d]{3}).*Mentor(\d)_*", filename)
    
    if not match or len(match.groups()) != 2:
        raise Exception("Bad filename: " + filename)
    
    mission_id = match.group(1)
    mentor_id = match.group(2)
        
    if mentor_id not in mentor_dict or mission_id not in mission_dict:
        raise Exception("Mission or mentor not supported filename: " + filename)
        
    mentor_name = mentor_dict[mentor_id]
    mission_name = mission_dict[mission_id]

    dialogue_json = get_dialogue_json(dialog_path)
    dialogue_ids, dialogue_texts = get_vide_dialogues(dialogue_json)
    
    table_location = "./planilhas/{}.xlsx".format(mission_name)
    table_dict = get_table_dict(table_location)
    
    dialogue_location = get_dialogue_location(table_dict, dialogue_texts)
    
    video_paths = list()
    vars_to_be_added = defaultdict(list) #get information to be added to the dialogs
    for d_id, coord in zip(dialogue_ids, dialogue_location):
        video_path = "/".join([mission_name, mentor_name, "{}.vp8".format(coord)])
        video_paths.append(video_path)
        vars_to_be_added[d_id].append(video_path)
        #dialogue_json = add_dialogue_extravar(dialogue_json, d_id, {'LibrasVideoPath': video_path})

    for d_id, paths in vars_to_be_added.items():
        for i, v_path in enumerate(paths):
            dialogue_json = add_dialogue_extravar(dialogue_json, d_id, {'LibrasVideoPath{}'.format(i): v_path})
    
    debug_table = pd.DataFrame()
    debug_table['DialogId'] = dialogue_ids
    debug_table['Dialogs'] = dialogue_texts
    debug_table['ClosestDialog'] = [table_dict[k] for k in dialogue_location]
    debug_table['ClosestDialogCoord'] = dialogue_location
    debug_table['LibrasVideoPath'] = video_paths
    #debug_table['Mission'] = mission_name
    #debug_table['Mentor'] = mentor_name
    #debug_table['MissionId'] = mission_id
    #debug_table['MentorId'] = mentor_id
    
    return dialogue_json, debug_table


if __name__ == "__main__":
    dialog_path = sys.argv[1]
    dialogue_json, debug_table = populate_dialogue_with_videos(dialog_path)

    filename = os.path.split(dialog_path)[-1]

    debug_table.to_excel("debug_tables/{}.xlsx".format(filename), index=False)

    with open(dialog_path, "w", encoding="latin") as f:
        json.dump(dialogue_json, f)

