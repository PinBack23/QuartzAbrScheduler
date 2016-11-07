var apiRoutes = (function () {
    var lsRootUrl = 'http://' + location.host + '/api/';

    var URLS = {
        ApiGetSchedulerIsStartet: undefined,
        ApiGetSchedulerStart: undefined,
        ApiGetSchedulerStop: undefined,
        ApiPostSchedulerUploadJobs: undefined,
        ApiGetReloadJobs: undefined,

        ApiGetJobs: undefined,
        ApiGetJobCount: undefined,
        ApiPutPauseJob: undefined,
        ApiPutToggleJob: undefined,
        ApiGetTemplates: undefined,
        ApiPutAddJobRelation: undefined,
        ApiGetJobRelations: undefined,
        ApiDeleteJobRelation: undefined,
        ApiGetCountJobExecutionFailed: undefined,
        ApiGetCountJobExecutionOk: undefined,
        ApiGetJobExecutions: undefined,

        ApiGetTriggers: undefined,
        ApiGetTriggerCount: undefined,
        ApiPutPauseTrigger: undefined,
        ApiPutToggleTrigger: undefined,
        ApiPutAddTriggerRelation: undefined,

        ApiGetAlerts: undefined,

        ApiGetLogs: undefined,
        ApiGetLogContent: undefined,
        ApiPostDownloadLogFile: undefined
    };

    URLS.ApiGetSchedulerIsStartet = lsRootUrl + "Scheduler/GetIsStartet";
    URLS.ApiGetSchedulerStart = lsRootUrl + "Scheduler/GetStartScheduler";
    URLS.ApiGetSchedulerStop = lsRootUrl + "Scheduler/GetStopScheduler";
    URLS.ApiPostSchedulerUploadJobs = lsRootUrl + "Scheduler/PostUploadJobs";
    URLS.ApiGetReloadJobs = lsRootUrl + "Scheduler/GetReloadJobs";
    
    URLS.ApiGetJobs = lsRootUrl + "Job/GetJobs";
    URLS.ApiGetJobCount = lsRootUrl + "Job/GetCount";
    URLS.ApiPutPauseJob = lsRootUrl + "Job/PutPause";
    URLS.ApiPutToggleJob = lsRootUrl + "Job/PutToggle";
    URLS.ApiGetTemplates = lsRootUrl + "Job/GetTemplates";
    URLS.ApiPutAddJobRelation = lsRootUrl + "Job/PutAddRelation";
    URLS.ApiGetJobRelations = lsRootUrl + "Job/GetJobRelations";
    URLS.ApiDeleteJobRelation = lsRootUrl + "Job/DeleteJobRelation";
    URLS.ApiGetCountJobExecutionFailed = lsRootUrl + "Job/GetCountJobExecutionFailed";
    URLS.ApiGetCountJobExecutionOk = lsRootUrl + "Job/GetCountJobExecutionOk";
    URLS.ApiGetJobExecutions = lsRootUrl + "Job/GetJobExecutions";

    URLS.ApiGetTriggers = lsRootUrl + "Trigger/GetTriggers";
    URLS.ApiGetTriggerCount = lsRootUrl + "Trigger/GetCount";
    URLS.ApiPutPauseTrigger = lsRootUrl + "Trigger/PutPause";
    URLS.ApiPutToggleTrigger = lsRootUrl + "Trigger/PutToggle";
    URLS.ApiPutAddTriggerRelation = lsRootUrl + "Trigger/PutAddRelation";
    
    URLS.ApiGetAlerts = lsRootUrl + "Alert/GetAlerts";

    URLS.ApiGetLogs = lsRootUrl + "Log/GetLogs";
    URLS.ApiGetLogContent = lsRootUrl + "Log/GetLogContent";
    URLS.ApiPostDownloadLogFile = lsRootUrl + "Log/PostDownloadLogFile";
    
    return {
        URLS: URLS
    };
}());