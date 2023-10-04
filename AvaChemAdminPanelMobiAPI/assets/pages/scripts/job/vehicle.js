function addVehicle(vehicleDTO, enumActionTypes, grpVehicleError_id, spnVehicleError_id) {
    try {
        var hidVehicleValues_id = '#VehicleValues';

        if (!vehicleDTO || !enumActionTypes) return;

        try {
            vehicleDTO = JSON.parse(vehicleDTO);

            if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
            if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
        } catch (_e) {
            return;
        }

        // int ID
        // string Number
        // string Model
        // int JobVehicleID
        // int? ActionType (enum DataActionTypes - enumActionTypes)
        var newVehicle = vehicleDTO;
        var newVehiclesText = $(hidVehicleValues_id).val() || JSON.stringify([]);
        var isExist = 0;
        var vehicles = [];

        try {
            vehicles = JSON.parse(newVehiclesText);
        } catch (ex) { }


        // Check exist or not
        for (var i = 0; i < vehicles.length; i++) {
            if (vehicles[i].ID == newVehicle.ID) {
                if (vehicles[i].ActionType === enumActionTypes.indexOf("Hide")) {
                    newVehicle.ActionType = enumActionTypes.indexOf("Show");

                    // Remove the old - then have to break the loop
                    vehicles.splice(i, 1);
                    isExist = 0;
                } else {
                    isExist = 1;
                }

                // IMPORTANT
                break;
            }
        }

        if (isExist === 0) {
            if (!newVehicle.ActionType) newVehicle.ActionType = enumActionTypes.indexOf("Create");
            vehicles.push(newVehicle);
            newVehiclesText = JSON.stringify(vehicles);

            if ($('#vehicle_' + newVehicle.ID).css("display") == "none") {
                $('#vehicle_' + newVehicle.ID).show();
            } else {
                var html = $('#tbdVehicle').html();

                html += "<tr id='vehicle_" + newVehicle.ID + "'>";
                html += "     <td id='vehicle_" + newVehicle.ID + "_Number'>" + newVehicle.Number + "</td>";
                html += "     <td id='vehicle_" + newVehicle.ID + "_Model'>" + newVehicle.Model + "</td>";
                html += "     <td class='text-center'>";
                html += "       <button type='button' class='btn btn-danger btn-sm' onclick='DeleteVehicle_Click(\"" + newVehicle.ID + "\")'><i class='fa fa-times' aria-hidden='true'></i></button>";
                html += "     </td>";
                html += "</tr>";

                $('#tbdVehicle').html(html);
            }
        } else {
            $(grpVehicleError_id).show();
            $(spnVehicleError_id).text('This vehicle already exists');
            return;
        }

        $(hidVehicleValues_id).val(newVehiclesText);

        $(grpVehicleError_id).hide();
        $(spnVehicleError_id).text('');

        //return { vehicleID: +newVehicle.ID, vehicleRoleID: +newVehicle.RoleID };
    } catch (e) {
    }
}

function deleteVehicle(vehicleID, enumActionTypes) {
    var hidVehicleValues_id = '#VehicleValues';

    var vehiclesText = $(hidVehicleValues_id).val();
    var vehicles = [];
    var newVehicle = {
        ID: vehicleID
    };
    try {
        vehicles = JSON.parse(vehiclesText);

        if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
        if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
    } catch (ex) {
    }

    for (var i = 0; i < vehicles.length; i++) {
        if (vehicles[i].ID == newVehicle.ID) {
            vehicles[i].ActionType = enumActionTypes.indexOf("Hide");
            vehicles.splice(i, 1, vehicles[i]);
            newVehicle = vehicles[i];
            // IMPORTANT
            break;
        }
    }

    var newVehiclesText = JSON.stringify(vehicles);
    $(hidVehicleValues_id).val(newVehiclesText);

    $('#vehicle_' + vehicleID).remove();

    //return { vehicleID: +newVehicle.ID, vehicleRoleID: +newVehicle.RoleID };
}