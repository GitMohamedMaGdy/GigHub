var GigsController = function (attendanceServices) {
    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);

    };
    var button;

    var toggleAttendance = function (e) {
        button = $(e.target);
        var gigId = button.attr("date-gig-id");
        if (button.hasClass("btn-default"))
            attendanceServices.createAttendance(gigId, done, fail);
        else
            attendanceServices.deleteAttendance(gigId, done, fail);
    };



    var fail = function () {
        alert("something error");

    };

    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);

    };

    return {
        init: init
    }
}(AttendanceService);

