<%@ page import="br.ufscar.sead.loa.antartica.remar.Quizz" %>



<div class="fieldcontain ${hasErrors(bean: quizzInstance, field: 'correctAnswer', 'error')} required">
	<label for="correctAnswer">
		<g:message code="quizz.correctAnswer.label" default="Correct Answer" />
		<span class="required-indicator">*</span>
	</label>
	<g:select name="correctAnswer" from="${0..3}" class="range" required="" value="${fieldValue(bean: quizzInstance, field: 'correctAnswer')}"/>

</div>

<div class="fieldcontain ${hasErrors(bean: quizzInstance, field: 'answers', 'error')} required">
	<label for="answers">
		<g:message code="quizz.answers.label" default="Answers" />
		<span class="required-indicator">*</span>
	</label>
	

</div>

<div class="fieldcontain ${hasErrors(bean: quizzInstance, field: 'ownerId', 'error')} required">
	<label for="ownerId">
		<g:message code="quizz.ownerId.label" default="Owner Id" />
		<span class="required-indicator">*</span>
	</label>
	<g:field name="ownerId" type="number" value="${quizzInstance.ownerId}" required=""/>

</div>

<div class="fieldcontain ${hasErrors(bean: quizzInstance, field: 'taskId', 'error')} required">
	<label for="taskId">
		<g:message code="quizz.taskId.label" default="Task Id" />
		<span class="required-indicator">*</span>
	</label>
	<g:textField name="taskId" required="" value="${quizzInstance?.taskId}"/>

</div>

<div class="fieldcontain ${hasErrors(bean: quizzInstance, field: 'title', 'error')} required">
	<label for="title">
		<g:message code="quizz.title.label" default="Title" />
		<span class="required-indicator">*</span>
	</label>
	<g:textField name="title" required="" value="${quizzInstance?.title}"/>

</div>

