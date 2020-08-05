function validateForm() {
    var name = $("#FirstName").val();
    var lastname = $("#LastName").val();
    var date = $("#Date").val();
    var phoneNumber = $("#PhoneNumber").val();
    const reg = /^([0-9]{9})$/;
    if (name == "") {
        return false;
    }
    if (lastname == "") {
        return false;
    }
    if (date == "") {
        return false;
    }
    if (phoneNumber == "") {
        return false;
    }
    if (!reg.test(phoneNumber)) {
        alert("Zły nr telefonu");
        return false;
    }
}