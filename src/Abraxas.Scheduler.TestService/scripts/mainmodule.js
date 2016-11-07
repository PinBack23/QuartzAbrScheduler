/// <reference path="jquery-2.1.3.js" />
/// <reference path="angular.js" />
/// <reference path="abrhelper.js" />
/// <reference path="apiroutes.js" />
/// <reference path="jquery.dataTables.js" />
var mainmodule = angular.module('mainmodule', ['ui.router']);

mainmodule.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/overview');

    $stateProvider
        .state('overview', {
            url: '/overview',
            templateUrl: 'Pages/OverviewPage.html',
            controller: "overviewController"
        })
        .state('jobtemplates', {
            url: '/jobtemplates',
            templateUrl: 'Pages/JobTemplatesPage.html',
            controller: "jobTemplatesController"
        })
        .state('jobrelations', {
            url: '/jobrelations',
            templateUrl: 'Pages/JobRelationPage.html',
            controller: "jobRelationController"
        })
        .state('jobexecution', {
            url: '/jobexecution',
            templateUrl: 'Pages/JobExecutionPage.html',
            controller: "jobExecutionController"
        })
        .state('alerts', {
            url: '/alerts',
            templateUrl: 'Pages/AlertPage.html',
            controller: "alertController"
        })
        .state('logs', {
            url: '/logs',
            templateUrl: 'Pages/LogPage.html',
            controller: "logController"
        });
});

mainmodule.controller('mainController', ['$scope', function ($scope) {
    $scope.data = {};

    $scope.ladeAlerts = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetAlerts, { "pnTake": 5 })
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.alerts = result;
                });
            }).fail(function (error) {
                alert("Konnte Alerts nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.ladeAlerts();
}]);

mainmodule.controller('overviewController', ['$scope', function ($scope) {
    $scope.data = {};

    $scope.ladeJobs = function () {
        $scope.data.triggers = undefined;
        abrHelper.getJson(apiRoutes.URLS.ApiGetJobs)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.jobs = result;
                });
            }).fail(function (error) {
                alert("Konnte Jobs nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.ladeTiggers = function () {
        $scope.data.jobs = undefined;
        abrHelper.getJson(apiRoutes.URLS.ApiGetTriggers)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.triggers = result;
                });
            }).fail(function (error) {
                alert("Konnte Triggers nicht lesen:\n" + angular.fromJson(error));
            });
    }

    $scope.refreshJobCount = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetJobCount)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.jobcount = result;
                });
            }).fail(function (error) {
                alert("Konnte Jobs nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.refreshJobCountExecutionFailed = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetCountJobExecutionFailed)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.jobcountexecutionfailed = result;
                });
            }).fail(function (error) {
                alert("Konnte Jobs nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.refreshJobCountExecutionOk = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetCountJobExecutionOk)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.jobcountexecutionok = result;
                });
            }).fail(function (error) {
                alert("Konnte Jobs nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.refreshTriggerCount = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetTriggerCount)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.triggercount = result;
                });
            }).fail(function (error) {
                alert("Konnte Jobs nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.checkSchedulerStatus = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetSchedulerIsStartet)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.schedulerStarted = result;
                    if ($scope.data.schedulerStarted) {
                        $scope.refreshJobCount();
                        $scope.refreshTriggerCount();
                        $scope.refreshJobCountExecutionFailed();
                        $scope.refreshJobCountExecutionOk();
                    }
                    else {
                        $scope.data = {};
                    }
                });
            }).fail(function (error) {
                alert("Konnte Status nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.toggleScheduler = function () {
        var lsApiUrl = apiRoutes.URLS.ApiGetSchedulerStart;
        if ($scope.data.schedulerStarted) {
            lsApiUrl = apiRoutes.URLS.ApiGetSchedulerStop;
        }
        abrHelper.getJson(lsApiUrl)
            .done(function () {
                $scope.checkSchedulerStatus();
            }).fail(function (error) {
                alert("Konnte Status nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.toggleTrigger = function (poTrigger) {
        abrHelper.putJson(apiRoutes.URLS.ApiPutToggleTrigger, poTrigger).done(function (response) {
            if ($scope.data.triggers) {
                $scope.ladeTiggers();
            } else if ($scope.data.jobs){
                $scope.ladeJobs();
            }
        }).fail(function (error) {
            alert("Trigger konnte nicht gestoppt werden:\n" + JSON.stringify(error));
        });
    };

    $scope.toggleJob = function (poJob) {
        abrHelper.putJson(apiRoutes.URLS.ApiPutToggleJob, poJob).done(function (response) {
            if ($scope.data.jobs) {
                $scope.ladeJobs();
            }
        }).fail(function (error) {
            alert("Jobs konnte nicht gestoppt werden:\n" + JSON.stringify(error));
        });
    };

    $scope.uploadJobs = function () {
        var loJobFile = $("#jobUpload")[0].files[0];
        if (loJobFile) {
            var loFormData = new FormData();
            loFormData.append("filename", loJobFile.name);
            loFormData.append("jobfile", loJobFile);

            var loAjaxRequest = $.ajax({
                cache: false,
                type: 'POST',
                url: apiRoutes.URLS.ApiPostSchedulerUploadJobs,
                contentType: false,
                processData: false,
                data: loFormData
            });

            loAjaxRequest.done(function (xhr, textStatus) {
                alert(textStatus);
            });
        }
    };

    $scope.prepareTriggerForJob = function (poJob) {
        $scope.data.newTriggername = "Trigger for " + poJob.Name;
        $scope.data.newNewcronexpression = "";
        $scope.data.newTriggerForJob = poJob;
    };

    //$scope.createNewTrigger = function () {
    //    var loTriggerRelation = {
    //        JobName: $scope.data.newTriggerForJob.Name,
    //        Group: $scope.data.newTriggerForJob.Group,
    //        TriggerName: $scope.data.newTriggername,
    //        CronExpression: $scope.data.newNewcronexpression
    //    };
    //    abrHelper.putJson(apiRoutes.URLS.ApiPutAddTriggerRelation, loTriggerRelation).done(function (response) {
    //        $scope.refreshTriggerCount();
    //    }).fail(function (error) {
    //        alert("Trigger konnte nicht gestoppt werden:\n" + JSON.stringify(error));
    //    });
    //};

    $scope.reloadJobs = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetReloadJobs)
            .done(function () {
                $scope.checkSchedulerStatus();
            }).fail(function (error) {
                alert("Konnte Status nicht lesen:\n" + angular.fromJson(error));
            });
    };

    $scope.checkSchedulerStatus();
}]);

mainmodule.controller('jobTemplatesController', ['$scope', function ($scope) {
    $scope.data = {};

    $scope.ladeTemplates = function () {
        $scope.data.addjob = undefined;
        abrHelper.getJson(apiRoutes.URLS.ApiGetTemplates)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.templates = result;
                });
            }).fail(function (error) {
                alert("Konnte Job-Templates nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.prepareNewJob = function (poJobTemplate) {
        $scope.data.addjob = { "Type": poJobTemplate.JobType, "Name": poJobTemplate.DefaultName, "Group": poJobTemplate.DefaultGroup, "CronExpression": "0/30 * * * * ?" };
        $scope.data.addjob.mandatoryConfigFields = [];
        $scope.data.addjob.optionalConfigFields = [];

        angular.forEach(poJobTemplate.MandatoryConfigFields, function (poField) {
            $scope.data.addjob.mandatoryConfigFields.push({ "Property": poField, "Value": undefined });
        });

        angular.forEach(poJobTemplate.OptionalConfigFields, function (poField) {
            $scope.data.addjob.optionalConfigFields.push({ "Property": poField, "Value": undefined });
        });
    };

    $scope.cancelNewJob = function () {
        $scope.ladeTemplates();
    };

    $scope.canSave = function () {
        if (angular.isUndefined($scope.data.addjob))
            return false;
        return $scope.data.addjob.Name && $scope.data.addjob.Group;
    };

    $scope.saveNewJob = function () {
        var loJobRelation = {};
        var loConfig = {};
        angular.forEach($scope.data.addjob.mandatoryConfigFields, function (poField) {
            loConfig[poField.Property] = poField.Value;
        });
        angular.forEach($scope.data.addjob.optionalConfigFields, function (poField) {
            loConfig[poField.Property] = poField.Value;
        });

        loJobRelation.JobName = $scope.data.addjob.Name;
        loJobRelation.Group = $scope.data.addjob.Group;
        loJobRelation.Type = $scope.data.addjob.Type;
        loJobRelation.CronExpression = $scope.data.addjob.CronExpression;
        loJobRelation.JsonConfig = angular.toJson(loConfig);

        abrHelper.putJson(apiRoutes.URLS.ApiPutAddJobRelation, loJobRelation).done(function (response) {
            $scope.ladeTemplates();
        }).fail(function (error) {
            alert("Job konnte nicht gestoppt werden:\n" + JSON.stringify(error));
        });

    };

    $scope.ladeTemplates();
}]);

mainmodule.controller('jobRelationController', ['$scope', function ($scope) {
    $scope.data = {};

    $scope.ladeRelations = function () {
        $scope.data.edit = undefined;
        abrHelper.getJson(apiRoutes.URLS.ApiGetJobRelations)
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.relations = result;
                });
            }).fail(function (error) {
                alert("Konnte Job-Relations nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.editJobRelation = function (poJobRelation) {
        $scope.data.edit = poJobRelation;
        $scope.data.edit.config = [];
        var loConfig = angular.fromJson(poJobRelation.JsonConfig);
        for (var lsProperty in loConfig) {
            if (loConfig.hasOwnProperty(lsProperty)) {
                $scope.data.edit.config.push({ "Property": lsProperty, "Value": loConfig[lsProperty] });
            }
        };
    };

    $scope.cancelEditJob = function () {
        $scope.ladeRelations();
    };

    $scope.canSave = function () {
        if (angular.isUndefined($scope.data.edit))
            return false;
        return $scope.data.edit.CronExpression;
    };

    $scope.saveRelation = function () {
        var loConfig = {};
        angular.forEach($scope.data.edit.config, function (poField) {
            loConfig[poField.Property] = poField.Value;
        });
        $scope.data.edit.JsonConfig = angular.toJson(loConfig);

        abrHelper.putJson(apiRoutes.URLS.ApiPutAddJobRelation, $scope.data.edit).done(function (response) {
            $scope.ladeRelations();
        }).fail(function (error) {
            alert("Job konnte nicht gestoppt werden:\n" + JSON.stringify(error));
        });
    };

    $scope.deleteRelation = function () {
        abrHelper.deleteJson(apiRoutes.URLS.ApiDeleteJobRelation, $scope.data.edit).done(function (response) {
            $scope.ladeRelations();
        }).fail(function (error) {
            alert("Job konnte nicht gelöscht werden:\n" + JSON.stringify(error));
        });
    };

    $scope.ladeRelations();
}]);

mainmodule.controller('alertController', ['$scope', function ($scope) {
    $scope.data = {};

    $scope.ladeAlerts = function () {
        abrHelper.getJson(apiRoutes.URLS.ApiGetAlerts)
            .done(function (result) {

                $scope.$apply(function () {
                    $scope.data.alerts = result;
                });

                setTimeout(function () {
                    $('#alertTable').DataTable({
                        responsive: true
                    });
                    $scope.$apply(function () {
                        $scope.data.tableReady = true;
                    });
                }, 500);

            }).fail(function (error) {
                alert("Konnte Alerts nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.ladeAlerts();
}]);

mainmodule.controller('logController', ['$scope', function ($scope) {
    $scope.data = {};
    var loTable = undefined;

    $scope.ladeLogs = function () {
        $scope.data.logcontent = undefined;
        $scope.data.showLogEntry = undefined;
        $scope.data.tableReady = false;
        abrHelper.getJson(apiRoutes.URLS.ApiGetLogs)
            .done(function (result) {

                $scope.$apply(function () {
                    $scope.data.logfiles = result;
                });

                setTimeout(function () {
                    if (!loTable) {
                        loTable = $('#logTable').DataTable({
                            responsive: true
                        });
                    }
                    else {
                        //loTable.fnClearTable(0);
                        //loTable.fnDraw();
                    }
                    $scope.$apply(function () {
                        $scope.data.tableReady = true;
                    });
                }, 500);

            }).fail(function (error) {
                alert("Konnte Logs nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.ladeContent = function (poLogEntry) {
        abrHelper.getJson(apiRoutes.URLS.ApiGetLogContent, { "psFileName": poLogEntry.FullName })
            .done(function (result) {
                $scope.$apply(function () {
                    $scope.data.logcontent = result;
                    $scope.data.showLogEntry = poLogEntry;
                });
            }).fail(function (error) {
                alert("Konnte Logs nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.downloadLog = function () {

        //window.open(apiRoutes.URLS.ApiGetDownloadLogFile + "?psFileName=" + $scope.data.showLogEntry.FullName, '_blank');

        //var loForm = document.createElement("form");
        //loForm.setAttribute("method", "post");
        //loForm.setAttribute("action", apiRoutes.URLS.ApiPostDownloadLogFile);
        //loForm.setAttribute("target", "view");

        //var loHiddenField = document.createElement("input");
        //loHiddenField.setAttribute("type", "hidden");
        //loHiddenField.setAttribute("name", "psFileName");
        //loHiddenField.setAttribute("value", $scope.data.showLogEntry.FullName);
        //loForm.appendChild(loHiddenField);
        //document.body.appendChild(loForm);

        //window.open('', 'view');
        //loForm.submit();

        abrHelper.openWindow("POST", apiRoutes.URLS.ApiPostDownloadLogFile, { "psFileName": $scope.data.showLogEntry.FullName });
        //abrHelper.openWindow("POST", apiRoutes.URLS.ApiPostDownloadLogFile, "=" + $scope.data.showLogEntry.FullName);
        //abrHelper.openWindow("POST", apiRoutes.URLS.ApiPostDownloadLogFile, { '': $scope.data.showLogEntry.FullName });
    };

    $scope.ladeLogs();
}]);

mainmodule.controller('jobExecutionController', ['$scope', function ($scope) {
    $scope.data = {};
    var loTable = undefined;

    $scope.ladeJobExecutions = function () {
        $scope.data.jobresult = undefined;
        abrHelper.getJson(apiRoutes.URLS.ApiGetJobExecutions)
            .done(function (result) {

                $scope.$apply(function () {
                    $scope.data.jobExecutions = result;
                });

                setTimeout(function () {

                    if (!loTable) {
                        loTable = $('#jobExecutionTable').DataTable({
                            responsive: true
                        });
                    }
                    else {
                        //loTable.fnClearTable(0);
                        //loTable.fnDraw();
                    }

                    $scope.$apply(function () {
                        $scope.data.tableReady = true;
                    });
                }, 500);

            }).fail(function (error) {
                alert("Konnte JobExecution nicht lesen:\n" + JSON.stringify(error));
            });
    };

    $scope.zeigeResult = function (poJobExecution) {
        $scope.data.jobresult = poJobExecution.Result;
        if (!angular.isString($scope.data.jobresult)) {
            $scope.data.jobresult = "No String Result from Job";
        }
    };

    $scope.ladeJobExecutions();
}]);

