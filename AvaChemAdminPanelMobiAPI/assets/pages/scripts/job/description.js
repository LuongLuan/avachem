function toggleJobDescription(cb, descriptionId, enumActionTypes) {
    try {
        var hidJDValues_id = '#JobDescriptionValues';

        if (!cb || !descriptionId) return;

        var jdText = $(hidJDValues_id).val();
        var jobDescriptions = [];
        try {
            jobDescriptions = JSON.parse(jdText);

            if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
            if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
        } catch (ex) {
        }

        var newJD = {
            ID: descriptionId,
            JobDescriptionID: 0
        }

        var isCheck = !!cb.checked;
        if (isCheck) {
            var isExist = 0;
            for (var i = 0; i < jobDescriptions.length; i++) {
                if (jobDescriptions[i].ID == newJD.ID) {
                    if (jobDescriptions[i].ActionType === enumActionTypes.indexOf("Hide")) {
                        newJD.ActionType = enumActionTypes.indexOf("Show");

                        // Remove the old - then have to break the loop
                        jobDescriptions.splice(i, 1);
                        isExist = 0;
                    } else {
                        isExist = 1;
                    }
                    // IMPORTANT
                    break;
                }
            }
            if (isExist === 0) {
                if (!newJD.ActionType) newJD.ActionType = enumActionTypes.indexOf("Create");
                jobDescriptions.push(newJD);
            }
        } else {
            for (var i = 0; i < jobDescriptions.length; i++) {
                if (jobDescriptions[i].ID == newJD.ID) {
                    jobDescriptions[i].ActionType = enumActionTypes.indexOf("Hide");
                    jobDescriptions.splice(i, 1, jobDescriptions[i]);
                    // IMPORTANT
                    break;
                }
            }
        }

        var newJobDescriptionsText = JSON.stringify(jobDescriptions);
        $(hidJDValues_id).val(newJobDescriptionsText);
    } catch (e) {
    }
}