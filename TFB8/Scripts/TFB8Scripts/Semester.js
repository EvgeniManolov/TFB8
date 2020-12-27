﻿$(document).ready(function () {
    semestersList();
    loadDisciplinesDropdown();
    $('option').mousedown(function (e) {
        e.preventDefault();
        $(this).prop('selected', !$(this).prop('selected'));
        return false;
    });
});

var Semester = {
    SemesterId: 0,
    StartDate: "",
    EndDate: "",
    Name : "",
    DisciplinesAsString: ""
}

$('option').mousedown(function (e) {
    e.preventDefault();
    $(this).prop('selected', !$(this).prop('selected'));
    return false;
});

function updateClick() {
    // Build semesters object from inputs
    semester = new Object();
    semester.SemesterId = $("#semesterid").val();
    semester.StartDate = $("#startdate").val();
    semester.EndDate = $("#enddate").val();
    semester.Name = $("#name").val();
    semester.DisciplinesAsString = $("#disciplinesAsString").val();


    if ($("#updateButton").text().trim() == "Add") {
        semesterAddAdd(semester);
    }
    else {
        semesterUpdate(semester);
    }
}

function loadDisciplinesDropdown() {
    // Call Web API to get a list of Semesters
    $.ajax({
        url: '/api/Discipline/',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            $.each(result, function (i) {
                $('#disciplinesDropdawn').append($('<option></option>').val(result[i].DisciplineId).html(result[i].DisciplineName));
            });
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function updateDisciplinesDropdown(Semester) {
    $('option').mousedown(function (e) {
        e.preventDefault();
        $(this).prop('selected', !$(this).prop('selected'));
        return false;
    });
    var myList = document.getElementById("disciplinesDropdawn")
    var currentValue = document.getElementById("DisciplinesAsString").value;
    if (currentValue !== "") {
        document.getElementById("DisciplinesAsString").value = currentValue + ", " + myList.options[myList.selectedIndex].text
    } else {
        document.getElementById("DisciplinesAsString").value = myList.options[myList.selectedIndex].text
    }
}


function semesterAdd(semester) {
    // Call Web API to add a new Semester
    $.ajax({
        url: "/api/Semester",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(semester),
        success: function (semester) {
            semesterAddSuccess(semester);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function semesterAddSuccess(Semester) {
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);

    semestersList();
    formClear();
}

function deleteRow() {
    row.parentNode.removeChild(row);
};

// Clear form fields
function formClear() {
    $("#semestername").val("");
    $("#professorname").val("");
}

function semestersList() {
    // Call Web API to get a list of Semesters
    $.ajax({
        url: '/api/Semester/',
        type: 'GET',
        dataType: 'json',
        success: function (Semesters) {
            semesterListSuccess(Semesters);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display all Semesters returned from Web API call
function semesterListSuccess(semesters) {
    // Iterate over the collection of data
    $.each(semesters,
        function (index, semester) {
            // Add a row to the Semester table
            semesterAddRow(semester);
        });
}

// Add Semester row to <table>
function semesterAddRow(semester) {
    if ($("#semesterTable tbody").length == 0) {
        $("#semesterTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#semesterTable tbody").append(
        semesterBuildTableRow(semester));
}

// Build a <tr> for a row of table data
function semesterBuildTableRow(semester) {
    var result = "<tr>" +
        "<td>" +
        "<button type='button' " +
        "onclick='semesterGet(this);' " +
        "class='btn btn-default' " +
        "data-id='" + semester.SemesterId + "'>" +
        "<i class='fa fa-pencil'></i>" +
        "</button>" +
        "</td>" +
        "<td>" + semester.Name + "</td>" +
        "<td>" + semester.StartDate + "</td>" +
        "<td>" + semester.EndDate + "</td>" +
        "<td>" + semester.DisciplinesAsString + "</td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='semesterDelete(this);' " +
        "class='btn btn-default' " +
        "data-id='" + semester.SemesterId + "'>" +
        "<span class='fa fa-remove' />" +
        "</button>" +
        "</td>" +
        "</tr>";



    return result;
}

function addClick() {
    formClear();
}


// Handle exceptions from AJAX calls
function handleException(request, message, error) {
    var msg = "";

    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.statusText + "\n";
    if (request.responseJSON != null) {
        msg += "Message" + request.responseJSON.Message + "\n";
    }

    alert(msg);
}