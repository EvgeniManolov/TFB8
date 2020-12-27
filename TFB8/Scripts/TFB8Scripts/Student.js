﻿$(document).ready(function () {
    studentsList();
    loadSemestersDropdown();
});

var Student = {
    StudentId: 0,
    Name: "",
    Surname: "",
    DateOfBirth: "",
    Semesters: []
}

function updateClick() {
    // Build student object from inputs
    student = new Object();
    student.StudentId = $("#studentid").val();
    student.Name = $("#name").val();
    student.Surname = $("#surname").val();
    student.DateOfBirth = $("#dateofbirth").val();
    student.Semesters = [];

    var semestersIds = $('select#semestersDropdown').val();

    for (var i = 0; i < semestersIds.length; i++) {
        var semester = { SemesterId: semestersIds[i] };
        student.Semesters.push(semester);
    }


    if ($("#updateButton").text().trim() == "Add") {
        studentAdd(student);
    }
    else {
        studentUpdate(student);
    }
}


function loadSemestersDropdown() {
    // Call Web API to get a list of Semesters
    $.ajax({
        url: '/api/Semester/',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            $.each(result, function (i) {
                $('#semestersDropdown').append($('<option></option>').val(result[i].SemesterId).html(result[i].Name));
            });
            $('option').mousedown(function (e) {
                e.preventDefault();
                $(this).prop('selected', !$(this).prop('selected'));
                return false;
            });
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function studentAdd(student) {
    if (student.Name === "") {
        error("Name is requered!")
    } else if (student.Surname === "") {
        error("Surname name is requered!")
    } else {
        // Call Web API to add a new Student
        $.ajax({
            url: "/api/Student",
            type: 'POST',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(student),
            success: function (student) {
                studentAddSuccess(student);
            },
            error: function (request, message, error) {
                handleException(request, message, error);
            }
        });
    }
}

function success(data) {
    alert(data);
}
function error(data) {
    alert(data);
}

function studentAddSuccess(student) {
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);

    studentsList();
    formClear();
}



// Clear form fields
function formClear() {
    $("#name").val("");
    $("#surname").val("");
    $("#dateofbirth").val("");
    $('#semestersDropdown option').prop('selected', false);
}


function studentsList() {
    // Call Web API to get a list of Students
    $.ajax({
        url: '/api/Student/',
        type: 'GET',
        dataType: 'json',
        success: function (students) {
            studentListSuccess(students);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

// Display all Students returned from Web API call
function studentListSuccess(students) {
    // Iterate over the collection of data
    $.each(students,
        function (index, student) {
            // Add a row to the Student table
            studentAddRow(student);
        });
}

// Add Student row to <table>
function studentAddRow(student) {
    if ($("#studentTable tbody").length == 0) {
        $("#studentTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#studentTable tbody").append(
        studentBuildTableRow(student));
}

// Build a <tr> for a row of table data
function studentBuildTableRow(student) {
    var result = "<tr>" +
        "<td>" +
        "<button type='button' " +
        "onclick='studentGet(this);' " +
        "class='btn btn-default' " +
        "data-id='" + student.StudentId + "'>" +
        "<i class='fa fa-pencil'></i>" +
        "</button>" +
        "</td>" +
        "<td>" + student.Name + "</td>" +
        "<td>" + student.Surname + "</td>" +
        "<td>" + student.DateOfBirth + "</td>" +
        "<td>" + student.CurrentSemester.Name + "</td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='fillMarks(this);' " +
        "class='btn btn-default' " +
        "data-semester-id='" + student.CurrentSemester.SemesterId + "'" +
        " data-id='" + student.StudentId + "'>" +
        "<i class='fa fa-address-book'></i>" +
        "</button>" +
        "</td>" +
        "<td>" +
        "<button type='button' " +
        "onclick='studentDelete(this);' " +
        "class='btn btn-default' " +
        "data-id='" + student.StudentId + "'>" +
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
    msg += "Text: " + request.responseText + "\n";
    if (request.responseJSON != null) {
        msg += "Message" + request.responseJSON.Message + "\n";
    }

    alert(msg);
}