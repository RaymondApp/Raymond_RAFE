
$(function () {
    $(document).on('focus', '.select2-selection.select2-selection--single', function (e) {
        $(this).closest(".select2-container").siblings('select:enabled').select2('open');
    });

    $("input[type=text]").attr("autocomplete", "off");
    onlyNumber();
    if ($(".lbl-success").length > 0) {
        swalSuccess("Success!", $(".lbl-success").text());
    }
    if ($(".lbl-error").length > 0) {
        swalSuccess("Cancelled", $(".lbl-error").text());
    }

    $('.cls-award-history').hide();
    $('.select2').select2();

    deleteInitialize();
    dataTableInitialize();
    if (parseInt($('#NominationID').val()) == 0) {
        ChangeAwardCategory();
    }
    else {
        ShowEmpSectionSet();
    }
    //SetQuarter();
    $(".loading").hide();

    $('input:radio[name=AwardCategory]').change(function () {
        $(".loading").show();
        GetAwardtype();
        ChangeAwardCategory();
    });

    $('#AwardType').change(function () {
        $(".loading").show();
        SetAwardSections();
    });

    $('#AwardYear').change(function () {
        //$(".loading").show();
        //SetQuarter();
    });

    $('.cls-award-history').click(function () {
        //Commented for not required IN RAFE
        //$(".loading").show();
        //ShowAwardHistory($(this).data("id"), this, $(this).data("place"));
    });

    DefaultEmpSection();
    

    //For Change Password from any where
    if ($("#frm_changepass").length > 0) {
        //reset form value on modal hide
        $('#chngPassModal').on('hide.bs.modal', function () {
            $('#frm_changepass')[0].reset();
        });
        //post change password form and get responce 
        //frist validate the form then post
        $('#frm_changepass').on('submit', function (e) {
            // if the validator does not prevent form submit
            if (!e.isDefaultPrevented()) {
                var URL = $("#hdnChangeURl").val();
                $(".loading").show();
                $("#chngPassModal").hide();
                var form_data = $(this).serialize(); //Encode form elements for submission
                $.ajax({
                    url: URL,
                    type: "POST",
                    data: form_data,
                    success: function (result) {
                        if (result == true) {
                            $("#chngPassModal").modal('hide');
                            swalSuccessAndReload("Logged In!", "You have successfully Logged In!",'');

                            $(".loading").hide();
                            $('#frm_changepass')[0].reset();
                        }
                        else {
                            $('#frm_changepass')[0].reset();
                            $(".loading").hide();
                            swalFail("Login failed!",'User dose not exist under your department.');
                            $("#chngPassModal").show();
                        }
                    },
                    error: function (jqXHR, exception) {
                        error(jqXHR, exception);
                        $(".loading").hide();
                        $("#chngPassModal").show();
                    },
                    failure: function (xhr) {
                        console.log(xhr);
                        $(".loading").hide();
                    }
                })
                return false;
            }
        });
        //End change password form post
    }
    //End Change Password
    if ($("#hdnnomenu").length > 0) {
        $("body").addClass("sidebar-collapse");
    }
});
function dataTableInitialize() {
    $('.dg-datatable').DataTable({
        'paging': true,
        'lengthChange': true,
        'searching': true,
        'ordering': true,
        'info': true,
        'autoWidth': true
    })
}

function deleteInitialize() {
    $(".is-delete").click(function () {
        var URL = $('#hyp_dashboard').attr('href') + "/" + $(this).data("rel") + "?id=" + $(this).data("id");
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover this record!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then(function (isConfirm) {
            if (isConfirm) {
                $(".loading").show();
                $.ajax({
                    url: URL,
                    type: "POST",
                    headers: {
                        'VerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    success: function (result) {
                        $(".loading").hide();
                        if (result == true) {
                            swalSuccessAndReload("Congrats!", "Record successfully deleted!", '')
                        }
                        else {
                            swalFail("Request fail!", result);
                        }
                    },
                    error: function (xhr) {
                        $(".loading").hide();
                    }
                });

            } else {
                swalFail("Cancelled", "Your record is safe :)");
                // swal("Cancelled", "Your record is safe :)");
            }
        });
    });
}
//Different Different alert box
function swalSuccessAndReload(vTitle, vMessage, url) {
    swal({
        title: vTitle,
        text: vMessage,
        icon: "success",
    }).then(function (isConfirm) {
        if (url == '') {
            window.location.reload()
        }
        else {
            window.open(url, "_self");
        }
    });
    //$(".swal-title").css('background-color', '#dff0d8').css('color', '#3c763d');
    //$('.swal-button').css('background-color', '#5cb85c');
}

function swalSuccess(vTitle, vMessage) {
    swal({
        title: vTitle,
        text: vMessage,
        icon: "success",
    });
    //$(".swal-title").css('background-color', '#dff0d8').css('color', '#3c763d');
    //$('.swal-button').css('background-color', '#5cb85c');
}

function swalFail(vTitle, vMessage) {
    swal({
        title: vTitle,
        text: vMessage,
        icon: "error",
    });
    //$(".swal-title").css('background-color', '#f2dede').css('color', '#a94442');
    //$('.swal-button').css('background-color', '#d9534f');
}

function swalWarning(vTitle, vMessage) {
    swal({
        title: vTitle,
        text: vMessage,
        icon: "info",
        confirmButtonClass: "btn-danger",
    });
    //$(".swal-title").css('background-color', '#fcf8e3').css('color', '#8a6d3b');//
    //$('.swal-button').css('background-color', '#f0ad4e');
}
//End alert box

//allow only number to text box
//set the class only-number to that textbox and call function on document ready event
function onlyNumber() {
    $(".only-number").focus(function (event) {
        var number = $(this).val();
        if (number == '0') {
            $(this).val('');
        }
    });
    $(".only-number").blur(function (event) {
        var number = $(this).val();
        if (number == '') {
            $(this).val('0');
        }
    });


    $(".only-number").on("keypress paste", function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 46 || charCode > 57)) {

            return false;
        }
        return true;
    })
}
//End only number to text box

function ChangeAwardCategory() {
    if ($("input[name='AwardCategory']:checked").val() == '1') {     
        $("#btnAddEmp").hide();
        $("#btnRemoveEmp").hide();
    }
    else if ($("input[name='AwardCategory']:checked").val() == '2' || $("input[name='AwardCategory']:checked").val() == '3') {    
        $("#btnAddEmp").show();
        $("#btnRemoveEmp").hide();
    }
    else {   
        $("#btnRemoveEmp").hide();
        $("#btnAddEmp").show();
    }
    for ( var c = parseInt($('#hdnEmpCount').val()); c > 1; c = c-1) {
        HideEmpSection(c);
    }
    $('#hdnEmpCount').val(1);
    $('#ddlemployee1').val('');
    $('#ddlemployee1').select2('destroy');
    $('#ddlemployee1').select2();
    $(".user-autocomplete").removeClass("collapse").addClass("collapse").val('');
}

function readImage(input, output) {
    $("#selected_image").hide();
    if (input.files && input.files[0]) {
        if (input.files[0].size > 2000000) {
            alert("Image Size should not be greater than 1MB");
           
            return false;
        }
        if (input.files[0].type.indexOf("image") == -1) {
            alert("Invalid Type");
          
            return false;
        }
        $("#selected_image").show();
        $('.span-employeeimage').text('');
    }
    
}

function readVideo(input) {
    $("#selected_video").hide();
    if (input.files && input.files[0]) {
        if (input.files[0].size > 10000000) {
            alert("Video Size should not be greater than 10MB");           
            return false;
        }
        if (input.files[0].type.indexOf("video") == -1) {
            alert("Invalid Type");           
            return false;
        }
        $("#selected_video").show();
    }
}


function readAddFile(input) {
    $("#selected_addfile").hide();
    if (input.files && input.files[0]) {
        if (input.files[0].size > 2000000) {
            alert("Document size should not be greater than 2MB");           
            return false;
        }        
        $("#selected_addfile").show();
    }
}

function financeAddFile(input) {

   
    $("#financeFileName").hide();
    if (input.files && input.files[0]) {
        if (input.files[0].size > 2000000) {
            alert("Document size should not be greater than 2MB");
            return false;
        }
        $("#financeFileName").show();
    }
}

function removeImage(output) {
    $('#' + output).attr("src", "blank");
    $('#' + output).hide();
}

function ShowEmpSection() {
    sectionNo = parseInt($('#hdnEmpCount').val()) + 1;
    if (sectionNo < 11) {
        $('#divEmp_' + sectionNo).show();
        $('#ddlemployee' + sectionNo).val('');
        $('#ddlemployee' + sectionNo).attr('required', 'required');
        $('#ddlemployee' + sectionNo).val('');
        $("#flemp" + sectionNo).val(null);
        $('#hdnEmpCount').val(sectionNo)
        if (sectionNo == 10) {
            $("#btnAddEmp").hide();
        }
        else if (sectionNo == 2) {
            $("#btnRemoveEmp").show();
        }
    }
}

function HideEmpSection() {
    sectionNo = parseInt($('#hdnEmpCount').val());
    if (sectionNo > 1) {
        $('#divEmp_' + sectionNo).hide();
        $('#ddlemployee' + sectionNo).val('');
        $('#ddlemployee' + sectionNo).removeAttr('required');
        $('#ddlemployee' + sectionNo).val('');
        $('#ddlemployee' + sectionNo).select2('destroy');
        $('#ddlemployee' + sectionNo).select2();
        $('#txtOtheremployee' + sectionNo).val('');
        $('#txtOtheremployee' + sectionNo).removeAttr('required');
        $("#flemp" + sectionNo).val(null);
        sectionNo = parseInt($('#hdnEmpCount').val()) - 1;
        $('#hdnEmpCount').val(sectionNo)
        if (sectionNo == 1) {
            $("#btnRemoveEmp").hide();
        }
        else if (sectionNo == 9) {
            $("#btnAddEmp").show();
        }
    }
    checkAllEmpSelected();
}

function checkSameEmployee(input) {
    thisID = input.id;
    if ($("#AwardType").val() == '')
    {
        $("#btnSaveEmp").attr("disabled", "disabled");
        swalFail('Stop', 'Please first select award type.');
        //$("#AwardType").focus()

        $('#AwardType').trigger('focus');
        $('#' + thisID).val('');
        $('#' + thisID).select2('destroy');
        $('#' + thisID).select2();
        return
    }
    $("#btnSaveEmp").removeAttr("disabled");
    for (i = 1; i <= $('#hdnEmpCount').val() ; i++) {
        if (thisID != 'ddlemployee' + i && input.value != '') {
            if (input.value == $('#ddlemployee' + i).val() && input.value.toString().toLowerCase() !="other") {
                $("#btnSaveEmp").attr("disabled", "disabled");
                swalFail('Fail', 'You can not select same employee');
                $('#' + thisID).val('');
                $('#' + thisID).select2('destroy');
                $('#' + thisID).select2();
                return
            }
        }
    }
    if (input.value != '')
        CheckAlreadyNominated(input);
}

function checkAllEmpSelected() {
    flag = true;
    for (i = 2; i <= $('#hdnEmpCount').val() ; i++) {
        if ($('#ddlemployee' + i).val() == '') {
            flag = false;
            $("#btnSaveEmp").attr("disabled", "disabled");
            break;
        }
    }
    if (flag) {
        $("#btnSaveEmp").removeAttr("disabled");
    }
}

function DefaultEmpSection() {
    if ($("#NominationID").length > 0 && parseInt($("#NominationID").val()) == 0) {
        $("#btnRemoveEmp").hide();
        for (sectionNo = 2; sectionNo <= parseInt(10) ; sectionNo++) {
            $('#divEmp_' + sectionNo).hide();
            $('#ddlemployee' + sectionNo).val('');
            $('#ddlemployee' + sectionNo).removeAttr('required');
            $('#ddlemployee' + sectionNo).val('');
            $("#flemp" + sectionNo).val(null);
        }
    }
    else {
        if ($("#hdnEmpCount").length > 0 && parseInt($("#hdnEmpCount").val()) > 0 && parseInt($("input[name='AwardCategory']:checked").val()) == 2 && parseInt($('#isSubmitted').val()) == 0) {
            if (parseInt($("#hdnEmpCount").val()) < 6) {
                $("#btnAddEmp").show();
            } else if (parseInt($("#hdnEmpCount").val()) ==6) {
                $("#btnAddEmp").hide();
            }
            $("#btnRemoveEmp").show();
        }
    }



   
}

function GetAwardtype() {
    $.ajax({
        url: $("#URL").val(),
        type: "POST",
        data: { id: $("input[name='AwardCategory']:checked").val(), DataSection: "AwardType" },
        success: function (result) {
            console.log(result)
            $('#AwardType').children().remove();
            $('#AwardType').append($('<option></option>').val('').html('--Select type--'));
            $.each(result, function (i, obj) {
                $('#AwardType').append($('<option></option>').val(obj.AwardTypeID).html(obj.AwardType));
            });
            $('#AwardType').select2('destroy');
            $('#AwardType').select2();
            $(".loading").hide();
        },
        error: function (jqXHR, exception) {
            $(".loading").hide();
        },
        failure: function (xhr) {
            console.log(xhr);
            $(".loading").hide();
        }
    })
}

function GetEmployee() {
    $.ajax({
        url: $("#URL").val(),
        type: "POST",
        data: { id: $("#AwardType").val(), DataSection: "EmployeeForAward" },
        success: function (result) {
            console.log(result)
            $('#ddlemployee1').children().remove();
            $('#ddlemployee1').append($('<option></option>').val('').html('--Select Employee--'));
            $.each(result, function (i, obj) {
                $('#ddlemployee1').append($('<option></option>').val(obj.EmployeeID).html(obj.EmployeeName));
            });
            $('#ddlemployee1').select2('destroy');
            $('#ddlemployee1').select2();
            $(".loading").hide();

            ChangeAwardCategory();
        },
        error: function (jqXHR, exception) {
            $(".loading").hide();
        },
        failure: function (xhr) {
            console.log(xhr);
            $(".loading").hide();
        }
    })
}

function SetAwardSections() {
    if ($("#AwardType").val() != '') {
        $.ajax({
            url: $("#URL").val(),
            type: "POST",
            data: { id: $("#AwardType").val(), DataSection: "AwardSection" },
            success: function (response, status, xhr) {
                if (xhr.statusText == "OK") {
                    $("#divAwardSection").html(response);
                    $("#divAwardSection").show();
                    $("input[type=text]").removeAttr("autocomplete");
                    $("input[type=text]").attr("autocomplete", "off");
                }
                else {
                    swalFail('Error', xhr.statusText);
                }
                $(".loading").hide();
            },
            error: function (xhr) {
                $(".loading").hide();
            }
        });


        GetEmployee();
    }
    else {
    }
    $(".loading").hide();
}

function SetQuarter() {
    $('#AwardQuarter').children().remove();
    for (i = 1; i <= 4; i++) {
        $('#AwardQuarter').append($('<option></option>').val($('#AwardYear').val() + ' Q' + i).html($('#AwardYear').val() + ' Q' + i));
    }
    $('.loading').hide();
}

function CheckAlreadyNominated(input) {
    thisID = input.id;
    if ($("#AwardYear").val() != '' && $("#AwardQuarter").val() != '' && $("#AwardType").val() != '' && input.value != '' && input.value != 'other') {
        $.ajax({
            url: $("#UrlAlreadyNominated").val(),
            type: "POST",
            data: { awardYear: $("#AwardYear").val(), awardQuarter: $("#AwardQuarter").val(), awardTypeID: $("#AwardType").val(), nominee: input.value },
            success: function (response, status, xhr) {
                if (xhr.statusText == "OK") {
                    if (response) {
                        swalFail('Fail', 'employee is already nominated.');
                        $('#' + thisID).val('');
                        $('#' + thisID).select2('destroy');
                        $('#' + thisID).select2();
                    }
                    else {
                        GetAwardHistory(input);
                    }
                }
                $(".loading").hide();
                return true;
            },
            error: function (xhr) {
                $('#' + thisID).val('');
                $('#' + thisID).select2('destroy');
                $('#' + thisID).select2();
                $(".loading").hide();
                return true;
            }
        });
    }
    else if ($("#AwardYear").val()=='') {
        swalFail('Stop', 'Please first select award year.');
        $("#AwardYear").focus();
        $('#' + thisID).val('');
        $('#' + thisID).select2('destroy');
        $('#' + thisID).select2();
    }
    else if ($("#AwardQuarter").val() == '') {
        swalFail('Stop', 'Please first select award quarter.');
        $("#AwardQuarter").focus();
        $('#' + thisID).val('');
        $('#' + thisID).select2('destroy');
        $('#' + thisID).select2();
    }
    else if ($("#AwardType").val() == '') {
        swalFail('Stop', 'Please first select award type.');
        $("#AwardType").focus();
        $('#' + thisID).val('');
        $('#' + thisID).select2('destroy');
        $('#' + thisID).select2();
    }
}

function GetAwardHistory(input) {
    var strInput = input.value;
    
    if (strInput != '') {
        //Commented for not required IN RAFE

        //$(".loading").show();
        //if (strInput.toString().toLowerCase() == "other") {
        //    strInput = $($(input).attr("dataotherid")).val();
        //}

        //$.ajax({
        //    url: $("#URL").val(),
        //    type: "POST",
        //    data: { id: strInput, DataSection: 'AwardHistory' },
        //    success: function (response, status, xhr) {
        //        if (xhr.statusText == "OK") {
        //            console.log($("#" + input.id).attr("dataid"));
        //            $("#" + input.id).parent().parent().parent().find(".award-history").html(response);
        //            TextSHowHide();
        //        }
        //        else {
        //            swalFail('Error', xhr.statusText);
        //        }
        //        $(".loading").hide();
        //    },
        //    error: function (xhr) {
        //        $(".loading").hide();
        //    }
        //});
    }
    else {
    }
}

//Calls when form is in view mode 
function ShowAwardHistory( val, input, dataPlace) {    
    if (input != '') {
        //Commented for not required IN RAFE
        //$(".loading").show();
        //$.ajax({
        //    url: $("#URL").val(),
        //    type: "POST",
        //    data: { id: val, DataSection: 'AwardHistory' },
        //    success: function (response, status, xhr) {              
        //        if (xhr.statusText == "OK") {
        //            if (response != '') {
        //                $('#' + dataPlace).empty();
        //                $('#' + dataPlace).html(response);
        //                TextSHowHide();
        //            }
        //        }
        //        else {
        //            swalFail('Error', xhr.statusText);
        //        }
        //        $(".loading").hide();
        //    },
        //    error: function (xhr) {
        //        $(".loading").hide();
        //    }
        //});
    }
    else {
    }
}

function TextSHowHide() {
    //$('.btn-toggl').click(function () {
    //    $(this).text(function (i, old) {
    //        return old == 'Show history' ? 'Hide history' : 'Show history';
    //    });
    //});
}

function ShowEmpSectionSet() {
    for (i = 1; i <= parseInt($('#hdnEmpCount').val()) ; i++) {
        $('#divEmp_' + i).show();
        $('#ddlemployee' + i).attr('required', 'required');
    }
    for (i = 10; i > parseInt($('#hdnEmpCount').val()) ; i--)
    {
        $('#divEmp_' + i).hide();
        $('#ddlemployee' + i).removeAttr('required');
    }
}

function textCounter(field, field2, maxlimit) {

    if (field.value.length > maxlimit)
        field2.innerHTML = '(Exceed ' + (parseInt(field.value.length) - parseInt(maxlimit)) + ' char from max char limit)';
    else if (field.value.length > 0)
        field2.innerHTML = '(' + (parseInt(maxlimit) - parseInt(field.value.length)) + ' char remaining)';
    else if (field.value.length == 0)
        field2.innerHTML = '(Max ' + maxlimit + ' char)';
}