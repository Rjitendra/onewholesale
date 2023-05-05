$("#Resend").click(function (e) {
    var mobile = $("#mobile").val();
    var email = $("#email").val();
    var company = $("#company").val();

    $("#cover-spin").css("display", "block");
    $.ajax({
        type: 'POST',
        url: "/Registration/SendOtp",
        data: { name: company, mobile: mobile, email: email },
        dataType: "json",
        success: function (resultData) {
            if (resultData == 1) {
                alert("OTP has been sent again to your email-id and mobile no");
                $("#cover-spin").css("display", "none");
                return true;
            }
            else {
                alert("Something went wrong");
                $("#cover-spin").css("display", "none");
                e.preventDefault();
                return false;
            }
        }
    })
});
function CheckMobile() {
    var mobile = $("#mobile").val();
    $.ajax({
        type: 'POST',
        url: "/Registration/CheckMobile",
        data: { mobile: mobile },
        dataType: "json",
        success: function (resultData) {
            return resultData;
        }
    })
}
function CheckEmail() {
    var email = $("#email").val();
    $.ajax({
        type: 'POST',
        url: "/Registration/CheckEmail",
        data: { email: email },
        dataType: "json",
        success: function (resultData) {
            return resultData;
        }
    })
}
$("#verifyotp").click(function (e) {
    var mobile = $("#mobile").val();
    var email = $("#email").val();
    var otp = $("#otp").val();

    $("#cover-spin").css("display", "block");
    $.ajax({
        type: 'POST',
        url: "/Registration/VerifyOTP",
        data: { mobile: mobile, email: email, otp: otp },
        dataType: "json",
        success: function (resultData) {
            if (resultData == 1) {
                alert("Something not right");
                $("#cover-spin").css("display", "none");
                return true;
            }
            else if (resultData == 2) {
                alert("Invalid OTP");
                $("#cover-spin").css("display", "none");
                return true;
            }
            else if (resultData == 3) {
                alert("OTP has been expired");
                $("#cover-spin").css("display", "none");
                return true;
            }
            else if (resultData == 4) {
                alert("OTP has been verified successfully");
                $("#Resend").css("display", "none");
                $("#verifyotp").css("display", "none");
                $("#submitapp").css("display", "block");
                $("#cover-spin").css("display", "none");
                return true;
            }
            else {
                alert("Something went wrong");
                $("#cover-spin").css("display", "none");
                e.preventDefault();
                return false;
            }
        }
    })
});
$("#SaveData").click(function (e) {
    //debugger;
    var data = $("#defaultUnchecked");
    var data1 = $("#defaultUnchecked1");
    var pcount = 0;
    $('.form-control').removeClass('form-control1');
    if ($("#suppliertypeid").val() == "0") { $("#suppliertypeid").addClass('form-control1'); pcount = 1; }
    if ($("#company").val() == "") { $("#company").addClass('form-control1'); pcount = 1; }
    if ($("#mobile").val() == "") { $("#mobile").addClass('form-control1'); pcount = 1; }
    if ($("#email").val() == "") { $("#email").addClass('form-control1'); pcount = 1; }
    if ($("#address").val() == "") { $("#address").addClass('form-control1'); pcount = 1; }
    if ($("#state").val() == "") { $("#state").addClass('form-control1'); pcount = 1; }
    if ($("#dist").val() == "") { $("#dist").addClass('form-control1'); pcount = 1; }
    if ($("#area").val() == "") { $("#area").addClass('form-control1'); pcount = 1; }
    if ($("#pin").val() == "") { $("#pin").addClass('form-control1'); pcount = 1; }
    if ($("#landmark").val() == "") { $("#landmark").addClass('form-control1'); pcount = 1; }
    if ($("#pan").val() == "") { $("#pan").addClass('form-control1'); pcount = 1; }
    if ($("#tin").val() == "") { $("#tin").addClass('form-control1'); pcount = 1; }
    if ($("#license").val() == "") { $("#license").addClass('form-control1'); pcount = 1; }
    var checkmobile = CheckMobile();
    var checkemail = CheckEmail();
    if (pcount != 0) {
        alert("Please fill the required information");
        e.preventDefault();
        return false;
    }
    else if (checkmobile == 1) {
        alert("Same mobile no already registered");
        e.preventDefault();
        return false;
    }
    else if (checkemail == 1) {
        alert("Same email id already registered");
        e.preventDefault();
        return false;
    }
    else {
        if (data[0].checked == false) {
            alert("Please tick the terms & conditions");
            e.preventDefault();
            return false;
        }
        else if (data1[0].checked == false) {
            alert("Please tick the agreement box");
            e.preventDefault();
            return false;
        }
        else {
            var mobile = $("#mobile").val();
            var email = $("#email").val();
            var company = $("#company").val();
            $("#cover-spin").css("display", "block");
            $.ajax({
                type: 'POST',
                url: "/Registration/SendOtp",
                data: { name: company, mobile: mobile, email: email },
                dataType: "json",
                success: function (resultData) {
                    if (resultData == 1) {
                        alert("OTP has been sent to your email-id and mobile no");
                        $("#cover-spin").css("display", "none");
                        return true;
                    }
                    else {
                        alert("Something went wrong");
                        $("#cover-spin").css("display", "none");
                        e.preventDefault();
                        return false;
                    }
                }
            })
            //if (confirm("Are you sure want to save these detail ? ")) {
            //    $("#myform").submit(function (e) {
            //        $("#cover-spin").show();
            //    });
            //}
            //else {
            //    return false;
            //}
        }
    }
});
$("#submitapp").click(function (e) {

    if (confirm("Are you sure want to save these detail ? ")) {
        $("#myform").submit(function (e) {
            $("#cover-spin").show();
        });
    }
    else {
        return false;
    }
});
function BindImage(id) {
    //debugger;
    var output1 = document.getElementsByName('file' + id);

    var sizeInKB = output1[0].files[0].size / 1024;
    var sizeLimit = 500;
    if (sizeInKB > sizeLimit) {
        alert("Max file size 500KB");
        $('#file' + id).val('');
        return false;
    }
    else {
        var filedata = output1[0].files[0];
        var dataa = URL.createObjectURL(filedata);
        $('#link' + id).attr('href', dataa);
        $('#link' + id).show();
    }
}
function BindSupplier() {
    var data = $("#suppliertypeid option:selected").text();
    $("#suppliertype").val(data);
    var id = $("#suppliertypeid").val()
    $("#cover-spin").css("display", "block");
    $.ajax({
        type: 'POST',
        url: "/Registration/DocumentList",
        data: { id: id },
        dataType: "html",
        success: function (resultData) {
            //debugger;
            $("#docdiv").html(resultData);
            $("#cover-spin").css("display", "none");
        }
    })
}
function Remove(id) {
    $('#file' + id).val('');
    $('#link' + id).removeAttr('href');

}