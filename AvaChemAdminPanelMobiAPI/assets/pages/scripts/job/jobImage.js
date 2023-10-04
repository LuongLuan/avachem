function addJobImage(enumActionTypes, imgName, imgType, imgUrl, shouldReset) {
    try {
        var hidValues_id = imgType === 'before' ? '#BeforeImgValues' : '#AfterImgValues';
        var container_id = imgType === 'before' ? '#tbdBeforeImage' : '#tbdAfterImage';
        var prefix = imgType === 'before' ? 'beimg_' : 'afimg_';

        if (!enumActionTypes || !imgType || !imgUrl) return;
        
        var newImagesText = $(hidValues_id).val() || JSON.stringify([]);
        var images = [];
        try {
            images = JSON.parse(newImagesText);

            if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
            if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
        } catch (ex) { }

        if (shouldReset) {
            for (var i = 0; i < images.length; i++) {
                if (typeof images[i].ID === "number" && images[i].ID <= 0) deleteJobImage_Click(images[i].ID, imgType, enumActionTypes);
            }
            try {
                images = JSON.parse($(hidValues_id).val() || JSON.stringify([]));
            } catch (ex) {
            }
        }

        var newImage = {
            ID: 0,
            ImageUrl: imgUrl,
            ImageName: imgName
        };
        var lastNewId = 0;
        // Assign the lastNewId
        for (var i = 0; i < images.length; i++) {
            if (typeof images[i].ID === "number" && images[i].ID < 0) lastNewId = images[i].ID;
        }

        if (newImage.ID === 0) newImage.ID = lastNewId - 1;
        if (!newImage.ActionType) newImage.ActionType = enumActionTypes.indexOf("Create");
        images.push(newImage);
        newImagesText = JSON.stringify(images);

        if ($('#'+prefix+newImage.ID).css("display") == "none") {
            $('#'+prefix+newImage.ID).show();
        } else {
            var html = $(container_id).html();

            html += "<div id='"+prefix+newImage.ID+"' style='position: relative; margin: 0 12px 1.5rem 0; border: 1px dashed #808080;'>";
            html += "     <a href='"+newImage.ImageUrl+"' target='_blank'>";
            html += "           <div style='width: 100px; height: 100px; line-height: 100px;'>";
            html += "                   <img src='"+newImage.ImageUrl+"' style='width: 100%; max-height: 100%'/>";
            html += "           </div>";
            html += "     </a>";
            html += "     <button type='button' class='btn btn-danger btn-sm' style='position: absolute; z-index: 1; top: -1rem; right: -6px;' onclick='DeleteJobImage_Click(\"" + newImage.ID + "\",\"" + imgType + "\")'>";
            html += "           <i class='fa fa-times' aria-hidden='true'></i>";
            html += "     </button>";
            html += "</div>";

            $(container_id).html(html);
        }

        $(hidValues_id).val(newImagesText);
    } catch (e) {
    }
}

function deleteJobImage_Click(imgID, imgType, enumActionTypes) {
    var hidValues_id = imgType === 'before' ? '#BeforeImgValues' : '#AfterImgValues';
    var prefix = imgType === 'before' ? 'beimg_' : 'afimg_';

    var imagesText = $(hidValues_id).val();
    var images = [];
    try {
        images = JSON.parse(imagesText);

        if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
        if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
    } catch (ex) { }

    for (var i = 0; i < images.length; i++) {
        if (images[i].ID == imgID) {
            if (imgID <= 0) {
                images.splice(i, 1);
            } else {
                images[i].ActionType = enumActionTypes.indexOf("Hide");
                images.splice(i, 1, images[i]);
            }
            break;
        }
    }
    var newImagesText = JSON.stringify(images);
    $(hidValues_id).val(newImagesText);

    $('#' + prefix + imgID).remove();

    return images;
}