package br.ufscar.sead.loa.antartica.remar

import br.ufscar.sead.loa.remar.api.MongoHelper
import grails.plugin.springsecurity.annotation.Secured
import grails.util.Environment
import org.springframework.web.multipart.MultipartFile

import static org.springframework.http.HttpStatus.*
import grails.transaction.Transactional

@Secured(["isAuthenticated()"])
class QuizzController {
    
    def springSecurityService

    static allowedMethods = [save: "POST", update: "POST", delete: "DELETE", returnInstance: "GET"]

    def beforeInterceptor = [action: this.&check, only: ['index', 'exportQuizz','save', 'update', 'delete']]

    @Secured(['permitAll'])
    def index() { 

        if (params.t) {
            session.taskId = params.t
        }

        session.user = springSecurityService.currentUser

        def list = Quizz.findAllByOwnerId(session.user.id)

        if(list.size()==0){
            new Quizz(title: "Questão 1", answers: ["Alternativa A", "Alternativa B", "Alternativa C", "Alternativa D"], correctAnswer: 0, ownerId:  session.user.id, taskId: session.taskId).save flush: true
            new Quizz(title: "Questão 2", answers: ["Alternativa A", "Alternativa B", "Alternativa C", "Alternativa D"], correctAnswer: 0, ownerId:  session.user.id, taskId: session.taskId).save flush: true
            new Quizz(title: "Questão 3", answers: ["Alternativa A", "Alternativa B", "Alternativa C", "Alternativa D"], correctAnswer: 0, ownerId:  session.user.id, taskId: session.taskId).save flush: true
            new Quizz(title: "Questão 4", answers: ["Alternativa A", "Alternativa B", "Alternativa C", "Alternativa D"], correctAnswer: 0, ownerId:  session.user.id, taskId: session.taskId).save flush: true
        }

        list = Quizz.findAllByOwnerId(session.user.id)


        respond list, model: [faseBlocoGeloInstanceCount: Quizz.count(), errorImportQuestions:params.errorImportQuestions]

    }

    private check() {
        if (springSecurityService.isLoggedIn())
            session.user = springSecurityService.currentUser
        else {
            log.debug "Logout: session.user is NULL !"
            session.user = null
            redirect controller: "login", action: "index"

            return false
        }

    }

    def show(Quizz quizzInstance) {
        respond quizzInstance
    }

    def create() {
        respond new Quizz(params)
    }

    @Transactional
    def save(Quizz quizzInstance) {
        if (quizzInstance == null) {
            notFound()
            return
        }

        quizzInstance.answers[0]= params.answers1
        quizzInstance.answers[1]= params.answers2
        quizzInstance.answers[2]= params.answers3
        quizzInstance.answers[3]= params.answers4
        quizzInstance.ownerId = session.user.id as long
        quizzInstance.taskId = session.taskId as String
        quizzInstance.save flush:true

        redirect(action: "index")
    }

    def edit(Quizz quizzInstance) {
        respond quizzInstance
    }

    @Transactional
    def update() {
        Quizz quizzInstance = Quizz.findById(Integer.parseInt(params.quizzID))
        quizzInstance.title = params.title
        quizzInstance.answers[0]= params.answers1
        quizzInstance.answers[1]= params.answers2
        quizzInstance.answers[2]= params.answers3
        quizzInstance.answers[3]= params.answers4
        quizzInstance.correctAnswer = Integer.parseInt(params.correctAnswer)
        quizzInstance.ownerId = session.user.id as long
        quizzInstance.taskId = session.taskId as String
        quizzInstance.save flush:true

        redirect action: "index"
    }

    @Transactional
    def delete(Quizz quizzInstance) {

        if (quizzInstance == null) {
            notFound()
            return
        }

        quizzInstance.delete flush:true
        render "delete OK"
    }

    protected void notFound() {
        request.withFormat {
            form multipartForm {
                flash.message = message(code: 'default.not.found.message', args: [message(code: 'quizz.label', default: 'Quizz'), params.id])
                redirect action: "index", method: "GET"
            }
            '*'{ render status: NOT_FOUND }
        }
    }

    @Secured(['permitAll'])
    def returnInstance(Quizz quizzInstance){
        if (quizzInstance == null) {
            //notFound()
            render "null"
        }
        else{
            render quizzInstance.title + "%@!" +
                    quizzInstance.answers[0] + "%@!" +
                    quizzInstance.answers[1] + "%@!" +
                    quizzInstance.answers[2] + "%@!" +
                    quizzInstance.answers[3] + "%@!" +
                    quizzInstance.correctAnswer + "%@!" +
                    quizzInstance.id
        }

    }

    @Transactional
    def exportQuizz(){
        //popula a lista de questoes a partir do ID de cada uma
        ArrayList<Integer> list_quizzId = new ArrayList<Integer>() ;
        ArrayList<Quizz> quizzInstanceList = new ArrayList<Quizz>();
        list_quizzId.addAll(params.list_id);
        for (int i=0; i<list_quizzId.size();i++)
            quizzInstanceList.add(Quizz.findById(list_quizzId[i]));

        //cria o arquivo json
        createJsonFile("quizz.json", quizzInstanceList)

        // Finds the created file path
        def folder = servletContext.getRealPath("/data/${springSecurityService.currentUser.id}/${session.taskId}")
        String id = MongoHelper.putFile("${folder}/quizz.json")


        def port = request.serverPort
        if (Environment.current == Environment.DEVELOPMENT) {
            port = 8080
        }

        // Updates current task to 'completed' status
        render  "http://${request.serverName}:${port}/process/task/complete/${session.taskId}?files=${id}"


    }

    void createJsonFile(String fileName, ArrayList<Quizz> questionList){
        def dataPath = servletContext.getRealPath("/data")
        def instancePath = new File("${dataPath}/${springSecurityService.currentUser.id}/${session.taskId}")
        instancePath.mkdirs()

        File file = new File("$instancePath/"+fileName);
        def bw = new BufferedWriter(new OutputStreamWriter(
                new FileOutputStream(file), "UTF-8"))

        bw.write("{\n")
        bw.write("\t\"quantidadeQuestoes\": [\"" + questionList.size() + "\"],\n")
        for(def i=0; i<questionList.size();i++){
            bw.write("\t\"" + (i+1) + "\": [\"" + questionList[i].title.replace("\"","\\\"") + "\", ")
            bw.write("\""+ questionList[i].answers[0].replace("\"","\\\"") +"\", " + "\""+ questionList[i].answers[1].replace("\"","\\\"") +"\", ")
            bw.write("\""+ questionList[i].answers[2].replace("\"","\\\"") +"\", ")
            switch(questionList[i].correctAnswer){
                case 0:
                    bw.write("\"A\"]")
                    break;
                case 1:
                    bw.write("\"B\"]")
                    break;
                case 2:
                    bw.write("\"C\"]")
                    break;
                case 2:
                    bw.write("\"D\"]")
                    break;
                default:
                    println("Erro! Alternativa correta inválida")
            }
            if(i<questionList.size()-1)
                bw.write(",")
            bw.write("\n")
        }
        bw.write("}");
        bw.close();
    }

    @Transactional
    def generateQuestions(){
        MultipartFile csv = params.csv
        def error = false

        csv.inputStream.toCsvReader([ 'separatorChar': ';', 'charset':'UTF-8' ]).eachLine { row ->
            if(row.size() == 5) {
                println row
                Quizz quizzInstance = new Quizz()
                quizzInstance.title = row[0] ?: "NA";
                quizzInstance.answers[0] = row[1] ?: "NA";
                quizzInstance.answers[1] = row[2] ?: "NA";
                quizzInstance.answers[2] = row[3] ?: "NA";
                String correct = row[4] ?: "NA";
                quizzInstance.correctAnswer =  (correct.toInteger() - 1)
                quizzInstance.taskId = session.taskId as String
                quizzInstance.ownerId = session.user.id as long
                quizzInstance.save flush: true
            } else {
                error = true
            }
        }
        redirect(action: index(), params: [errorImportQuestions:error])
    }

    def exportCSV(){
        /* Função que exporta as questões selecionadas para um arquivo .csv genérico.
           O arquivo .csv gerado será compatível com os modelos Escola Mágica, Forca e Responda Se Puder.
           O arquivo gerado possui os seguintes campos na ordem correspondente:
           Nível, Pergunta, Alternativa1, Alternativa2, Alternativa3, Alternativa4, Alternativa5, Alternativa Correta, Dica, Tema.
           O campo Dica é correspondente ao modelo Responda Se Puder e o campo Tema ao modelo Forca.
           O separador do arquivo .csv gerado é o ";" (ponto e vírgula)
        */

        ArrayList<Integer> list_questionId = new ArrayList<Integer>() ;
        ArrayList<Quizz> quizzInstanceList = new ArrayList<Quizz>();
        list_questionId.addAll(params.list_id);
        for (int i=0; i<list_questionId.size();i++){
            quizzInstanceList.add(Quizz.findById(list_questionId[i]));

        }

        def dataPath = servletContext.getRealPath("/samples")
        def instancePath = new File("${dataPath}/export")
        instancePath.mkdirs()
        log.debug instancePath

        def fw = new BufferedWriter(new OutputStreamWriter(
                new FileOutputStream("$instancePath/exportQuestions.csv"), "UTF-8"))
        for(int i=0; i<quizzInstanceList.size();i++){
            fw.write(quizzInstanceList.getAt(i).title + ";" + quizzInstanceList.getAt(i).answers[0] + ";" + quizzInstanceList.getAt(i).answers[1] + ";" +
                    quizzInstanceList.getAt(i).answers[2] + ";" + quizzInstanceList.getAt(i).answers[3] + ";" +(quizzInstanceList.getAt(i).correctAnswer +1) + "\n" )
        }
        fw.close()

        def port = request.serverPort
        if (Environment.current == Environment.DEVELOPMENT) {
            port = 8080
        }

        render "/antartica/samples/export/exportQuestions.csv"
    }

}
