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
    // Call Web API to get a list of students
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

function studentUpdate(student) {
    var url = "/api/Student/" + student.StudentId;

    // Call Web API to update Student
    $.ajax({
        url: url,
        type: 'PUT',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(student),
        success: function (responseMessage) {
            studentUpdateSuccess();
            success(responseMessage);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}

function studentUpdateSuccess() {
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);

    studentsList();
    formClear();
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

function studentGet(ctl) {
    // Get student id from data- attribute
    var id = $(ctl).data("id");

    // Store student id in hidden field
    $("#studentid").val(id);

    // Call Web API to get a student
    $.ajax({
        url: "/api/student/" + id,
        type: 'GET',
        dataType: 'json',
        success: function (student) {
            studentToFields(student);

            // Change Update Button Text
            $("#updateButton").text("Update");
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}


function studentToFields(student) {
    $("#name").val(student.Name);
    var dateofbirth = createDate(student.DateOfBirth)
    $("#surname").val(student.Surname);
    $("#dateofbirth").val(dateofbirth);
    $('#semestersDropdown option:selected').removeAttr('selected');
    var options = document.getElementById('semestersDropdown').options;

    for (i = 0; i < options.length; i++) {
        for (r = 0; r < student.Semesters.length; r++) {
            var semesterId = parseInt(student.Semesters[r].SemesterId);
            var optiondId = parseInt(options[i].value);
            if (semesterId === optiondId) {
                $('#semestersDropdown > option[value="' + optiondId + '"]').attr("selected", "selected")
            }
        }
    }
}


function createDate(unformatedDate) {
    var newDate = new Date(unformatedDate);
    var day = newDate.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var year = newDate.getFullYear();
    var month = parseInt(newDate.getMonth()) + 1;
    if (month < 10) {
        month = "0" + month;
    }
    var date = year + "-" + month + "-" + day;
    return date;
}

function studentsList() {
    // Call Web API to get a list of Disciplines
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
            // Add a row to the student table
            studentAddRow(student);
        });
}

function getScores(ctl) {
    $('.pop-outer').fadeIn('slow');
    var id = $(ctl).data("id");

    $.ajax({
        url: "/api/score/" + id,
        type: 'GET',
        dataType: 'json',
        success: function (disciplinesscore) {
            scoresToFields(disciplinesscore);
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });
}
// Display all Score returned from Web API call
function scoresToFields(disciplinesscores) {
    // Iterate over the collection of data
    $.each(disciplinesscores,
        function (index, discipline) {
            // Add a row to the score table
            scoreAddRow(discipline);
        });
}

function scoreAddRow(discipline) {
    if ($("#scoresTable tbody").length == 0) {
        $("#scoresTable").append("<tbody></tbody>");
    }

    // Append row to <table>
    $("#scoresTable tbody").append(
        scoreBuildTableRow(discipline));
}

function saveScores() {
    var arr = $('tr[data-id]:not([data-id=""])').map(function () {
        return {
            id: $(this).data('id'),
            value: $(this).find('span').text()
        };
    }).get();
    var tds = $(".mark");

    var scores = []
    for (i = 0; i < tds.length; i++) {
        score = new Object();
        score.ScoreId = arr[i].id;
        score.Mark = tds[i].innerText;
        scores.push(score);
    }

    $.ajax({
        url: "/api/Score",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(scores),
        success: function (scores) {
            scoresAddSucccess();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
        }
    });

}

function scoresAddSucccess() {
    $('#scoresTable tbody').empty();
    var row = document.getElementsByTagName('tbody')[0];
    row.parentNode.removeChild(row);
    closeModal();


    studentsList();
    formClear();
}

// Build a <tr> for a row of table data
function scoreBuildTableRow(discipline) {
    var mark = '';
    if (discipline.Mark != null) {
        mark = discipline.Mark;
    }
    var result = "<tr data-id='" + discipline.ScoreId + "'>" +
        "<td>" + discipline.Discipline.DisciplineName + "</td>" +
        "<td class='mark' " + "contenteditable='true'>" + mark + "</td>" +
        "</tr>";


    return result;
}

function closeModal() {
    $('.pop-outer').fadeOut('slow');
    $('#scoresTable tbody').empty();
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
        "onclick='getScores(this);' " +
        "class='btn btn-default' " +
        "data-semester-id='" + student.CurrentSemester.SemesterId + "'" +
        " data-id='" + student.ScoreId + "'>" +
        "<i class='fa fa-address-book'></i>" +
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