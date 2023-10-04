function getUrlVars() {
    var url = location.search;
    var vars = [], hash;
    var hashes = url && url.includes('?') ? url.slice(url.indexOf('?') + 1).split('&') : [];
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function dateToYMD(date) {
    var d = date.getDate();
    var m = date.getMonth() + 1; //Month from 0 to 11
    var y = date.getFullYear();
    return '' + y + '-' + (m <= 9 ? '0' + m : m) + '-' + (d <= 9 ? '0' + d : d);
}

function ajaxRequest(payload) {
    // let { method, url, body, beforeSend, success, error, complete } = payload
    payload = payload || {};
    return $.ajax({
        type: payload.method,
        url: payload.url,
        contentType: "application/json",
        //accepts: "application/json",
        data: JSON.stringify({ data: JSON.stringify(payload.body) }),
        dataType: payload.dataType || 'json', // (default : Intelligent Guess (xml, json, script, or html))
        beforeSend: function (xhr, settings) {
            if (typeof payload.beforeSend !== 'function') return;
            payload.beforeSend(xhr, settings);
        }
    }).done(function (data, textStatus, jqXHR) {
        try {
            if (typeof payload.success !== 'function') return;
            var res = data.d ? data.d : data;
            try {
                res = res && typeof res === 'string' ? JSON.parse(res) : res;
            } catch (e) {

            }
            payload.success({
                data: res,
                status: jqXHR.status,
                textStatus,
                jqXHR
            })
        } catch (e) {
            if (typeof payload.error !== 'function') return;
            payload.error({
                jqXHR,
                status: jqXHR.status,
                textStatus,
                error: e
            });
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        if (typeof payload.error !== 'function') return;
        payload.error({
            jqXHR,
            textStatus,
            error: errorThrown
        });
    }).always(payload.complete);
}


function getUploadedFileURL(event) {
    try {
        if (!event.target.files || !event.target.files[0]) return "";
        var uploadedFile = event.target.files[0];
        var uploadedFileUrl = URL.createObjectURL(uploadedFile);
        return uploadedFileUrl || "";
    } catch (e) {
        return "";
    }
}

function b64toBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;

    var byteCharacters = atob(b64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);

        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}

function removeDuplicates(originalArray, prop) {
    var newArray = [];
    var lookupObject = {};

    for (var i in originalArray) {
        lookupObject[originalArray[i][prop]] = originalArray[i];
    }

    for (i in lookupObject) {
        newArray.push(lookupObject[i]);
    }
    return newArray;
}

function getDuplicates(originalArray, prop) {
    var lookupArr = originalArray.reduce(function (a, e) {
        a[e[prop]] = ++a[e[prop]] || 0;
        return a;
    }, {});

    return originalArray.filter(function (e) { return lookupArr[e[prop]] });
}

function getSameItems(arr1, arr2, prop) {
    var result = arr1.filter(function (o1) {
        return arr2.some(function (o2) {
            return o1[prop] === o2[prop]; // return the ones with equal id
        });
    });
    return result;
}

// Document ready
$(function () {
    var uri = window.location.href.toString();
    if (uri.indexOf("?") > 0) {
        var params = getUrlVars();
        var cleanPr = (params.filter(function (p) { return p !== "message" })
            .map(function (p) { return p ? p + "=" + params[p] : "" })).join("&") || "";
        var cleanUri = uri.split('?')[0] + (cleanPr ? "?" + cleanPr : "")
        window.history.replaceState({}, document.title, cleanUri);
    }
});