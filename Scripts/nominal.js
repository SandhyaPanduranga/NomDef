function JSTest(val) {
    alert(val);
}

function hideUpdateCarButton() {

    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if (all[i].name == "UpdateCarButton") {
                all[i].disabled = true;
            }
            if (all[i].name == "DeleteCarButton") {
                all[i].disabled = true;
            }
        }
    }

}

function populateLodger(Company, FirstName, LastName, Address, Suburb, LodgerId) {
    var DisplayText = "";
    
    if (Company != "") {
        DisplayText += Company + "\r\n";
    }

    if (FirstName != "" || LastName != "") {
        DisplayText += FirstName + " " + LastName + "\r\n";
    }

    if (Address != "") {
        var AddressDisplay = Address.replace(/XXYYXX/g, "\r\n");
        DisplayText += AddressDisplay + "\r\n";
    }

    if (Suburb != "") {
        DisplayText += Suburb + "\r\n";
    }

    DisplayText = trim(DisplayText);
    window.opener.document.getElementById("MainContent_LoggedByInput").value = DisplayText;
    window.opener.document.getElementById("MainContent_LoggedById").value = LodgerId;
    self.close();
}

function trim(s) {
    s = s.replace(/(^\s*)|(\s*$)/gi, "");
    s = s.replace(/[ ]{2,}/gi, " ");
    s = s.replace(/\n /, "\n");
    return s;
}

function populateSuburb(postcode, suburb, LocationId) {
    window.opener.document.getElementById("MainContent_LocationInput").value = suburb + ", NSW, " + postcode;
    window.opener.document.getElementById("MainContent_LocationId").value = LocationId;
    self.close();
}
winOpened = 0;
function OpenWindow(strChildPageUrl){
    if (winOpened == 1) {
        checkIfStillOpen();
    }
    testwindow = window.open(strChildPageUrl, "Child", "width=400px,height=400px,top=0,left=0,scrollbars=1");
    winOpened = 1;
}


function OpenWindowLoggedBy(strChildPageUrl) {
    if (winOpened == 1) {
        checkIfStillOpen();
    }
    testwindow = window.open(strChildPageUrl, "Child", "width=500px,height=700px,top=0,left=0,scrollbars=1,status=1");
    winOpened = 1;
}

function checkIfStillOpen() {
    if (false == testwindow.closed) {
        testwindow.close();
    }
}

function updateMarketShareTotal() {
    var oForm = document.forms[0];
    var MyElements = oForm.elements;
    var totalShare = 0;
    for (var i = MyElements.length - 1; i >= 0; --i) {
        a = MyElements[i];
        if (a.type == "text") {
            totalShare = totalShare + parseFloat(a.value);
        }
    }
    document.getElementById("MainContent_HiddenTotal").value = totalShare;
    document.getElementById("TotalMarketShare").innerHTML = totalShare + "%";
}

function NumberDecimalOnlyEntry(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    } else if (e) {
        key = e.which;
    } else {
        return true;
    }

    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
        // numbers
    } else if ((("0123456789.").indexOf(keychar) > -1)) {
        return true;
        // decimal point jump
    } else if (dec && (keychar == ".")) {
        //myfield.form.elements[dec].focus();
        return true;
    } else {
        return false;
    }
}

function NumberOnlyEntry(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    } else if (e) {
        key = e.which;
    } else {
        return true;
    }

    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
        // numbers
    } else if ((("0123456789").indexOf(keychar) > -1)) {
        return true;
        // decimal point jump
    } else if (dec && (keychar == ".")) {
        //myfield.form.elements[dec].focus();
        return false;
    } else {
        return false;
    }
}

function relatedClaimEntry(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    } else if (e) {
        key = e.which;
    } else {
        return true;
    }

    keychar = String.fromCharCode(key);
    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27) || (key == 188) || (key == 96) || (key == 97) || (key == 98) || (key == 99) || (key == 100) || (key == 101) || (key == 102) || (key == 103) || (key == 104) || (key == 105)) {
        return true;
        // numbers
    } else if ((("0123456789ABCDEFGHIJKLMOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").indexOf(keychar) > -1)) {
        return true;
        // decimal point jump
    } else if (dec && (keychar == ".")) {
        //myfield.form.elements[dec].focus();
        return false;
    } else {
        return false;
    }
}

function RestrictAllEntry(myfield, e, IdField) {
    var keycode;
    if (window.event) // IE 
    {
        keycode = e.keyCode;
    }
    else if (e.which) // Netscape/FF/Op 
    {
        keycode = e.which;
    }
    if (keycode == 9) {
        return true;
    }
    if (keycode == 8 || keycode == 46) {
        myfield.value = "";
        if (IdField) {
            IdField.value = "";
        }
    }
    return false;  
}

function blurField(myfield) {
    myfield.blur();
}

function NoEntry(myfield, e) {
    return false;
}

function noTextEntry(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    } else if (e) {
        key = e.which;
    } else {
        return true;
    }

    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
        // numbers
    } else if ((("0123456789/\\-").indexOf(keychar) > -1)) {
        return true;
        // decimal point jump
    } else if (dec && (keychar == ".")) {
        //myfield.form.elements[dec].focus();
        return false;
    } else {
        return false;
    }
}

function numberPlateEntrySearch(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    } else if (e) {
        key = e.which;
    } else {
        return true;
    }

    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
        // numbers
    } else if ((("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,").indexOf(keychar) > -1)) {
        return true;
        // decimal point jump
    } else if (dec && (keychar == ".")) {
        //myfield.form.elements[dec].focus();
        return false;
    } else {
        return false;
    }
}

function numberPlateEntry(myfield, e, dec) {
    var key;
    var keychar;

    if (window.event) {
        key = window.event.keyCode;
    } else if (e) {
        key = e.which;
    } else {
        return true;
    }

    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;
        // numbers
    } else if ((("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").indexOf(keychar) > -1)) {
        return true;
        // decimal point jump
    } else if (dec && (keychar == ".")) {
        //myfield.form.elements[dec].focus();
        return false;
    } else {
        return false;
    }
}

function shiftFocus(fieldName) {
    var obj = document.getElementById(fieldName);
    obj.focus();
    //document.getElementById(fieldName).focus();
}

function deleteCar(carId) {
    document.getElementById("MainContent_deletecarId").value = carId;
    document.forms[0].submit();
}

function deleteDoc(docId) {
    document.getElementById("MainContent_deletedocId").value = docId;
    document.forms[0].submit();
}

function deleteNote(noteId) {
    document.getElementById("MainContent_deleteNoteId").value = noteId;
    document.forms[0].submit();
}

function deleteLetter(letterId) {
    document.getElementById("MainContent_deleteLetterId").value = letterId;
    document.forms[0].submit();
}

function deleteUser(UserId) {
    if (confirm("Are you sure you wish to delete this user")) {
        document.getElementById("MainContent_deleteUserId").value = UserId;
        document.forms[0].submit();
    }
}

function updateDoc(docId) {

    document.getElementById("MainContent_rowDispDoc_" + docId).style.display = "none";
    document.getElementById("MainContent_rowEditDoc_" + docId).style.display = "block";
    document.getElementById("MainContent_updatedocId").value = docId;

    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if (all[i].name == "UpdateDocButton") {
                all[i].disabled = true;
            }
            if (all[i].name == "DeleteDocButton") {
                all[i].disabled = true;
            }
        }
    }
}

function cancelUpdateDoc(docId) {
    document.getElementById("MainContent_rowDispDoc_" + docId).style.display = "block";
    document.getElementById("MainContent_rowEditDoc_" + docId).style.display = "none";
    document.getElementById("MainContent_updatedocId").value = "";

    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if (all[i].name == "UpdateDocButton") {
                all[i].disabled = false;
            }
            if (all[i].name == "DeleteDocButton") {
                all[i].disabled = false;
            }
        }
    }

}

function UpdateUser(userId) {
    document.getElementById("MainContent_rowDisp_" + userId).style.display = "none";
    document.getElementById("MainContent_rowEdit_" + userId).style.display = "block";
    document.getElementById("MainContent_updateUserId").value = userId;
    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if (all[i].name == "UpdateUserButton") {
                all[i].disabled = true;
            }
        }
    }
}

function CancelSaveUser(userId) {
    document.getElementById("MainContent_rowDisp_" + userId).style.display = "block";
    document.getElementById("MainContent_rowEdit_" + userId).style.display = "none";
    document.getElementById("MainContent_updateUserId").value = "";

    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if (all[i].name == "UpdateUserButton") {
                all[i].disabled = false;
            }
        }
    }
}


function updateCar(carId) {
    document.getElementById("MainContent_rowDisp_" + carId).style.display = "none";
    document.getElementById("MainContent_rowEdit_" + carId).style.display = "block";
    document.getElementById("MainContent_updatecarId").value = carId;

    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if(all[i].name == "UpdateCarButton"){
                all[i].disabled = true;
            }
            if (all[i].name == "DeleteCarButton") {
                all[i].disabled = true;
            }
        }
    }
}

function cancelUpdateCar(carId) {
    document.getElementById("MainContent_rowDisp_" + carId).style.display = "block";
    document.getElementById("MainContent_rowEdit_" + carId).style.display = "none";
    document.getElementById("MainContent_updatecarId").value = "";

    var all = document.forms[0];

    for (var i = 0; i < all.length; i++) {
        if (all[i].type.toLowerCase() == 'button') {
            if (all[i].name == "UpdateCarButton") {
                all[i].disabled = false;
            }
            if (all[i].name == "DeleteCarButton") {
                all[i].disabled = false;
            }
        }
    }

}



function getFolder() {
    return showModalDialog("folderDialog.html", "", "width:400px;height:400px;resizeable:yes;");
}


