/// <reference path="jquery-2.1.3.js" />
/// <reference path="angular.js" />
var abrHelper = (function () {

    var ERROR_CONST = {
        REQUIRED: "abrRequired",
        LENGTH: "abrLength",
        NOTANUMBER: "abrNotANumber",
        NUMBERFORMAT: "abrNumberFormat",
        NOTADATE: "abrNotADate",
        CUSTOM: "abrCustom"
    };

    var DEFAULTS = {
        NumberFormat: "0,0.00"
    };

    var loGetJson = function (psUrl, poJsonData) {
        $.ajaxSetup({ "cache": false });
        if (poJsonData) {
            return $.getJSON(psUrl, poJsonData);
        }
        else {
            return $.getJSON(psUrl);
        }
    };

    var loPostJson = function (psUrl, poJsonData) {
        return sendAjaxRequest(psUrl, "POST", poJsonData);
    };

    var loPutJson = function (psUrl, poJsonData) {
        return sendAjaxRequest(psUrl, "PUT", poJsonData);
    };

    var loDeleteJson = function (psUrl, poJsonData) {
        return sendAjaxRequest(psUrl, "DELETE", poJsonData);
    };

    var loDeleteRequest = function (psUrl, pnId) {
        $.ajaxSetup({ "cache": false });
        return $.ajax({
            data: { "_method": "delete" },
            dataType: 'script',
            type: "DELETE",
            url: psUrl + "/" + pnId
        });
    };

    var loOpenWindow = function (psMethode, psUrl, poData, psTarget) {
        var loForm = document.createElement("form");
        loForm.action = psUrl;
        loForm.method = psMethode;
        loForm.target = psTarget || "_self";
        if (poData) {
            for (var lsKey in poData) {
                var loInput = document.createElement("textarea");
                loInput.name = lsKey;
                loInput.value = typeof poData[lsKey] === "object" ? JSON.stringify(poData[lsKey]) : poData[lsKey];
                loForm.appendChild(loInput);
            }
        }
        loForm.style.display = "none";
        document.body.appendChild(loForm);
        loForm.submit();
    };

    //#region Private Funktionen

    function isDoubleClick(poEventargs) {
        var loComponent = poEventargs.component;
        var ldPrevClickTime = loComponent.lastClickTime;
        loComponent.lastClickTime = new Date();
        return ldPrevClickTime && (loComponent.lastClickTime - ldPrevClickTime < 300);
    }

    function sendAjaxRequest(psUrl, psType, poJsonData) {

        return $.ajax({
            cache: false,
            type: psType,
            url: psUrl,
            contentType: 'application/json',
            dataType: "json",
            data: JSON.stringify(poJsonData)
        });
    }

    //#endregion

    return {
        ERROR_CONST: ERROR_CONST,
        DEFAULTS: DEFAULTS,
        getJson: loGetJson,
        postJson: loPostJson,
        putJson: loPutJson,
        deleteJson: loDeleteJson,
        deleteRequest: loDeleteRequest,
        openWindow: loOpenWindow
    };
}());