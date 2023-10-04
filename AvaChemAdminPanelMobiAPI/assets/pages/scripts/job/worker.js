function addWorker(userDTO, roles, enumActionTypes, grpWorkerError_id, spnWorkerError_id) {
    try {
        var hidWorkerValues_id = '#WorkerValues';

        if (!userDTO || !roles || !enumActionTypes) return;

        try {
            if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
            if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
        } catch (_e) {
            return;
        }

        // int ID
        // string IDNumber
        // string Name
        // string Email
        // string Phone
        // int RoleID
        // int UserJobID
        // int? ActionType (enum DataActionTypes - enumActionTypes)
        var newWorker = userDTO;
        var newWorkersText = $(hidWorkerValues_id).val() || JSON.stringify([]);
        var isExist = 0;
        var workers = [];

        try {
            workers = JSON.parse(newWorkersText);
        } catch (ex) { }


        // Check exist or not
        for (var i = 0; i < workers.length; i++) {
            if (workers[i].ID == newWorker.ID) {
                if (workers[i].ActionType === enumActionTypes.indexOf("Hide")) {
                    newWorker.ActionType = enumActionTypes.indexOf("Show");

                    // Remove the old - then have to break the loop
                    workers.splice(i, 1);
                    isExist = 0;
                } else {
                    isExist = 1;
                }

                // IMPORTANT
                break;
            }
        }

        if (isExist === 0) {
            if (!newWorker.ActionType) newWorker.ActionType = enumActionTypes.indexOf("Create");
            workers.push(newWorker);
            newWorkersText = JSON.stringify(workers);

            if ($('#w_' + newWorker.ID).css("display") == "none") {
                $('#w_' + newWorker.ID).show();
            } else {
                var html = $('#tbdWorker').html();
                html += "<tr id='w_" + newWorker.ID + "'>";
                html += "     <td id='w_" + newWorker.ID + "_Name'>" + newWorker.Name + "</td>";
                html += "     <td id='w_" + newWorker.ID + "_IDNumber'>" + newWorker.IDNumber + "</td>";
                html += "     <td id='w_" + newWorker.ID + "_Phone' class='text-center'>" + newWorker.Phone + "</td>";
                html += "     <td id='w_" + newWorker.ID + "_Email'>" + newWorker.Email + "</td>";
                html += "     <td id='w_" + newWorker.ID + "_RoleID' class='text-center'>" + roles[newWorker.RoleID] + "</td>";
                html += "     <td class='text-center'>";
                html += "       <button type='button' class='btn btn-danger btn-sm' onclick='DeleteWorker_Click(\"" + newWorker.ID + "\")'><i class='fa fa-times' aria-hidden='true'></i></button>";
                html += "     </td>";
                html += "</tr>";
                $('#tbdWorker').html(html);
            }
        } else {
            $(grpWorkerError_id).show();
            $(spnWorkerError_id).text('This worker already exists');
            return;
        }

        $(hidWorkerValues_id).val(newWorkersText);

        $(grpWorkerError_id).hide();
        $(spnWorkerError_id).text('');

        //return { workerID: +newWorker.ID, workerRoleID: +newWorker.RoleID };
    } catch (e) {
    }
}

function deleteWorker(workerID, enumActionTypes) {
    var hidWorkerValues_id = '#WorkerValues';

    var workersText = $(hidWorkerValues_id).val();
    var workers = [];
    var newWorker = {
        ID: workerID,
        RoleID: 0
    };
    try {
        workers = JSON.parse(workersText);
        if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
        if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
    } catch (ex) {
    }

    for (var i = 0; i < workers.length; i++) {
        if (workers[i].ID == newWorker.ID) {
            workers[i].ActionType = enumActionTypes.indexOf("Hide");
            workers.splice(i, 1, workers[i]);
            newWorker = workers[i];
            // IMPORTANT
            break;
        }
    }

    var newWorkersText = JSON.stringify(workers);
    $(hidWorkerValues_id).val(newWorkersText);

    $('#w_' + newWorker.ID).remove();


    //return { workerID: +newWorker.ID, workerRoleID: +newWorker.RoleID };
}