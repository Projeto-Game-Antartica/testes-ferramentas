package br.ufscar.sead.loa.antartica.remar

class Quizz {
    String title
    String[] answers = new String[4]
    int correctAnswer

    long ownerId
    String taskId

    static constraints = {
        correctAnswer(range: 0..3)
    }
}