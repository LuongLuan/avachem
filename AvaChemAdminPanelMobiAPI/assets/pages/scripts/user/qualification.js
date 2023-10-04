function submitQ_Click(enumActionTypes) {
    var hidSubmitQID_id = '#SubmitQID';

    var qID = $(hidSubmitQID_id).val();
    if (!qID) addQ(enumActionTypes);
    else editQ(qID, enumActionTypes);
}

function addQ_Click() {
    var hidSubmitQID_id = '#SubmitQID';
    var tbxQName_id = '#tbxQName';
    var tbxQDateObtained_id = '#tbxQDateObtained';
    var tbxQExpiryDate_id = '#tbxQExpiryDate';
    var grpQError_id = '#grpQModalError';
    var spnQError_id = '#spnQModalError';

    $(hidSubmitQID_id).val('');
    $(tbxQName_id).val('');
    $(tbxQDateObtained_id).val('');
    $(tbxQExpiryDate_id).val('');
    $(grpQError_id).hide();
    $(spnQError_id).text('');
}
function addQ(enumActionTypes) {
    try {
        var hidQValues_id = '#QValues';
        var modUpdateQ_id = '#updateQualificationModal';

        var tbxQName_id = '#tbxQName';
        var tbxQDateObtained_id = '#tbxQDateObtained';
        var tbxQExpiryDate_id = '#tbxQExpiryDate';

        var grpQError_id = '#grpQModalError';
        var spnQError_id = '#spnQModalError';


        if (!$(tbxQName_id).val() || !$(tbxQDateObtained_id).val() || !$(tbxQExpiryDate_id).val()) return;


        var newQsText = $(hidQValues_id).val() || JSON.stringify([]);
        var qs = [];
        try {
            qs = JSON.parse(newQsText);

            if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
            if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
        } catch (ex) { }

        var newQ = {
            ID: 0,
            Name: $(tbxQName_id).val(),
            DateObtained: $(tbxQDateObtained_id).val(),
            ExpiryDate: $(tbxQExpiryDate_id).val(),
        };
        var isExist = 0;
        var lastNewId = 0;
        // Assign the lastNewId
        for (var i = 0; i < qs.length; i++) {
            if (typeof qs[i].ID === "number" && qs[i].ID < 0) lastNewId = qs[i].ID;
        }

        // Check exist or not
        // for (var i = 0; i < qs.length; i++) {
        //     if (qs[i].Name == newQ.Name) {
        //         if (qs[i].ActionType === enumActionTypes.indexOf("Hide")) {
        //             newQ.ActionType = enumActionTypes.indexOf("Show");
        //             if (typeof qs[i].ID === "number" && qs[i].ID > 0) newQ.ID = qs[i].ID;
        //             // Remove the old - then have to break the loop
        //             qs.splice(i, 1);
        //             isExist = 0;
        //         } else {
        //             isExist = 1;
        //         }
        //         // IMPORTANT
        //         break;
        //     }
        // }

        if (isExist === 0) {
            if (newQ.ID === 0) newQ.ID = lastNewId - 1;
            if (!newQ.ActionType) newQ.ActionType = enumActionTypes.indexOf("Create");
            qs.push(newQ);
            newQsText = JSON.stringify(qs);

            if ($('#q_' + newQ.ID).css("display") == "none") {
                $('#q_' + newQ.ID).show();
            } else {
                var html = $('#tbdQ').html();

                html += "<tr id='q_" + newQ.ID + "'>";
                html += "     <td id='q_" + newQ.ID + "_Name'>" + newQ.Name + "</td>";
                html += "     <td id='q_" + newQ.ID + "_DateObtained'>" + reformatDate(newQ.DateObtained) + "</td>";
                html += "     <td id='q_" + newQ.ID + "_ExpiryDate'>" + reformatDate(newQ.ExpiryDate) + "</td>";
                html += "     <td class='text-center'>";
                html += "       <button type='button' data-toggle='modal' data-target='"+modUpdateQ_id+"' class='btn btn-primary btn-sm' onclick='editQ_Click(\""+newQ.ID+"\")'><i class='fa fa-pencil' aria-hidden='true'></i></button>";
                html += "     </td>";
                html += "     <td class='text-center'>";
                html += "       <button type='button' class='btn btn-danger btn-sm' onclick='deleteQ_Click(\""+newQ.ID+"\","+JSON.stringify(enumActionTypes)+")'><i class='fa fa-times' aria-hidden='true'></i></button>";
                html += "     </td>";
                html += "</tr>";

                $('#tbdQ').html(html);
            }
        } else {
            $(grpQError_id).show();
            $(spnQError_id).text('This qualification name already exists');
            return;
        }

        $(hidQValues_id).val(newQsText);

        $(modUpdateQ_id).modal('hide');
        $(tbxQName_id).val('');
        $(tbxQDateObtained_id).val('');
        $(tbxQExpiryDate_id).val('');
        $(grpQError_id).hide();
        $(spnQError_id).text('');
    } catch (e) {
    }
}

function editQ_Click(qID) {
    var hidQValues_id = '#QValues';
    var modUpdateQ_id = '#updateQualificationModal';
    var hidSubmitQID_id = '#SubmitQID';

    var tbxQName_id = '#tbxQName';
    var tbxQDateObtained_id = '#tbxQDateObtained';
    var tbxQExpiryDate_id = '#tbxQExpiryDate';

    var grpQError_id = '#grpQModalError';
    var spnQError_id = '#spnQModalError';

    $(hidSubmitQID_id).val('');
    $(tbxQName_id).val('');
    $(tbxQDateObtained_id).val('');
    $(tbxQExpiryDate_id).val('');
    $(grpQError_id).hide();
    $(spnQError_id).text('');

    if (!qID) $(modUpdateQ_id).modal('hide');

    var qsText = $(hidQValues_id).val() || JSON.stringify([]);
    var qs = [];
    try {
        qs = JSON.parse(qsText);
    } catch (ex) { }

    for (var i = 0; i < qs.length; i++) {
        if (qs[i].ID == qID) {
            $(hidSubmitQID_id).val(qID);
            $(tbxQName_id).val(qs[i].Name);
            $(tbxQDateObtained_id).val(qs[i].DateObtained);
            $(tbxQExpiryDate_id).val(qs[i].ExpiryDate);
            break;
        }
    }
}
function editQ(qID, enumActionTypes) {
    var hidQValues_id = '#QValues';
    var modUpdateQ_id = '#updateQualificationModal';

    var tbxQName_id = '#tbxQName';
    var tbxQDateObtained_id = '#tbxQDateObtained';
    var tbxQExpiryDate_id = '#tbxQExpiryDate';

    var grpQError_id = '#grpQModalError';
    var spnQError_id = '#spnQModalError';


    if (!$(tbxQName_id).val() || !$(tbxQDateObtained_id).val() || !$(tbxQExpiryDate_id).val()) return;

    var qsText = $(hidQValues_id).val();
    var qs = [];
    try {
        qs = JSON.parse(qsText);

        if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
        if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
    } catch (ex) { }

    var isExist = 0;
    var newQ = {
        ID: qID,
        Name: $(tbxQName_id).val(),
        DateObtained: $(tbxQDateObtained_id).val(),
        ExpiryDate: $(tbxQExpiryDate_id).val(),
        ActionType: enumActionTypes.indexOf("Update")
    };

    var focusIndex = -1;
    for (var i = 0; i < qs.length; i++) {
        if (qs[i].ID == qID) {
            focusIndex = i;
        } 
        // else if (qs[i].Name == newQ.Name) isExist = 1;
    }
    if (isExist === 0) {
        qs.splice(focusIndex, 1, newQ);
        var newQsText = JSON.stringify(qs);
        $(hidQValues_id).val(newQsText);

        $('#q_' + newQ.ID + "_Name").html(newQ.Name);
        $('#q_' + newQ.ID + "_DateObtained").html(reformatDate(newQ.DateObtained));
        $('#q_' + newQ.ID + "_ExpiryDate").html(reformatDate(newQ.ExpiryDate));
    } else {
        $(grpQError_id).show();
        $(spnQError_id).text('This qualification name already exists');
        return;
    }

    $(modUpdateQ_id).modal('hide');
    $(tbxQName_id).val('');
    $(tbxQDateObtained_id).val('');
    $(tbxQExpiryDate_id).val('');
    $(grpQError_id).hide();
    $(spnQError_id).text('');
}

function deleteQ_Click(qID, enumActionTypes) {
    var hidQValues_id = '#QValues';

    var qsText = $(hidQValues_id).val();
    var qs = [];
    try {
        qs = JSON.parse(qsText);

        if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
        if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
    } catch (ex) { }

    for (var i = 0; i < qs.length; i++) {
        if (qs[i].ID == qID) {
            qs[i].ActionType = enumActionTypes.indexOf("Hide");
            qs.splice(i, 1, qs[i]);
            break;
        }
    }
    
    var newQsText = JSON.stringify(qs);
    $(hidQValues_id).val(newQsText);

    $('#q_' + qID).remove();
}

function reformatDate(dateStr) { // ex input "2010-01-18"  
    return dateStr.split('-').reverse().join('-'); //ex out: "18/01/10"
}