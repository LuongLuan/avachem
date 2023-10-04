function addCrew(userDTO, roles, enumActionTypes, grpCrewError_id, spnCrewError_id) {
    try {
        var hidCrewValues_id = '#CrewValues';

        if (!userDTO || !roles || !enumActionTypes) return;

        try {
            userDTO = JSON.parse(userDTO);

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
        // int UserOT_ID
        // int? ActionType (enum DataActionTypes - enumActionTypes)
        var newCrew = userDTO;
        var newCrewsText = $(hidCrewValues_id).val() || JSON.stringify([]);
        var isExist = 0;
        var crews = [];

        try {
            crews = JSON.parse(newCrewsText);
        } catch (ex) { }

        
        // Check exist or not
        for (var i = 0; i < crews.length; i++) {
            if (crews[i].ID == newCrew.ID) {
                if (crews[i].ActionType === enumActionTypes.indexOf("Hide")) {
                    newCrew.ActionType = enumActionTypes.indexOf("Show");

                    // Remove the old - then have to break the loop
                    crews.splice(i, 1);
                    isExist = 0;
                } else {
                    isExist = 1;
                }

                // IMPORTANT
                break;
            }
        }

        if (isExist === 0) {
            if (!newCrew.ActionType) newCrew.ActionType = enumActionTypes.indexOf("Create");
            crews.push(newCrew);
            newCrewsText = JSON.stringify(crews);

            if ($('#c_' + newCrew.ID).css("display") == "none") {
                $('#c_' + newCrew.ID).show();
            } else {
                var html = $('#tbdCrew').html();
                html += "<tr id='c_" + newCrew.ID + "'>";
                html += "     <td id='c_" + newCrew.ID + "_Name'>" + newCrew.Name + "</td>";
                html += "     <td id='c_" + newCrew.ID + "_IDNumber'>" + newCrew.IDNumber + "</td>";
                html += "     <td id='c_" + newCrew.ID + "_Phone' class='text-center'>" + newCrew.Phone + "</td>";
                html += "     <td id='c_" + newCrew.ID + "_Email'>" + newCrew.Email + "</td>";
                html += "     <td id='c_" + newCrew.ID + "_RoleID' class='text-center'>" + roles[newCrew.RoleID] + "</td>";
                html += "     <td class='text-center'>";
                html += "       <button type='button' class='btn btn-danger btn-sm' onclick='DeleteCrew_Click(\""+newCrew.ID+"\")'><i class='fa fa-times' aria-hidden='true'></i></button>";
                html += "     </td>";
                html += "</tr>";
                $('#tbdCrew').html(html);
            }
        } else {
            $(grpCrewError_id).show();
            $(spnCrewError_id).text('This crew already exists');
            return;
        }

        $(hidCrewValues_id).val(newCrewsText);

        $(grpCrewError_id).hide();
        $(spnCrewError_id).text('');

        return { crewID: +newCrew.ID, crewRoleID: +newCrew.RoleID };
    } catch (e) {
    }
}

function deleteCrew(crewID, enumActionTypes) {
    var hidCrewValues_id = '#CrewValues';
    
    var crewsText = $(hidCrewValues_id).val();
    var crews = [];
    var newCrew = {
        ID: crewID,
        RoleID: 0
    };
    try {
        crews = JSON.parse(crewsText);
        if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
        if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
    } catch (ex) {
    }

    for (var i = 0; i < crews.length; i++) {
        if (crews[i].ID == newCrew.ID) {
            crews[i].ActionType = enumActionTypes.indexOf("Hide");
            crews.splice(i, 1, crews[i]);
            newCrew = crews[i];
            // IMPORTANT
            break;
        }
    }
    
    var newCrewsText = JSON.stringify(crews);
    $(hidCrewValues_id).val(newCrewsText);

    $('#c_' + newCrew.ID).remove();

    
    return { crewID: +newCrew.ID, crewRoleID: +newCrew.RoleID };
}