var sessionTimeout = (function (sessionTimeout) {
    var progressBarId = "session-progress-bar";
    var timerId = "session-remaining-time";
    var interval = null;
    var minutes = 0;
    var redirectUrl = "";

    sessionTimeout.init = function (expirationMinutes, expirationRedirectUrl) {
        minutes = expirationMinutes;
        redirectUrl = expirationRedirectUrl;
    };

    sessionTimeout.start = function () {
        var totalSeconds = minutes * 60;
        var percentagePerSecond = 100 / totalSeconds;
        var remaining = totalSeconds - 1;
        interval = setInterval(function () {
            if (remaining > 0) {
                var width = Math.round(100 * percentagePerSecond * remaining) / 100;
                $('#' + progressBarId).css('width', width + '%');
                $('#' + timerId).html(formatTime(remaining));
                setProgressBarColor(width);
                remaining--;
            }
            else {
                window.location = redirectUrl;
            }
        }, 1000);
    };

    sessionTimeout.reset = function () {
        $.ajax({
            url: "/Login/RenewSession",
            method: "GET"
        }).done(function (html) {
            clearInterval(interval);
            $('#' + progressBarId).css('width','100%');
            $('#' + timerId).html(formatTime(minutes * 60));
            setProgressBarColor(100);
            sessionTimeout.start();
        });
    };

    function setProgressBarColor(percentage) {
        if (percentage > 49) {
            $('#' + progressBarId).addClass('progress-bar-success');
            $('#' + progressBarId).removeClass('progress-bar-warning');
            $('#' + progressBarId).removeClass('progress-bar-danger');
        }
        else if (percentage > 19) {
            $('#' + progressBarId).addClass('progress-bar-warning');
            $('#' + progressBarId).removeClass('progress-bar-danger');
            $('#' + progressBarId).removeClass('progress-bar-success');
        } else {
            $('#' + progressBarId).addClass('progress-bar-danger');
            $('#' + progressBarId).removeClass('progress-bar-warning');
            $('#' + progressBarId).removeClass('progress-bar-success');
        }
    };

    function formatTime(totalSeconds) {

        var hours = Math.floor(totalSeconds / 3600);
        var minutes = Math.floor((totalSeconds - (hours * 3600)) / 60);
        var seconds = totalSeconds - (hours * 3600) - (minutes * 60);

        if (hours === 0) {
            hours = '';
        }
        else if (hours < 10) {
            hours = "0" + hours;
        }
        if (minutes < 10) { minutes = "0" + minutes; }
        if (seconds < 10) { seconds = "0" + seconds; }

        if (hours === '') {
            return minutes + ':' + seconds;
        }
        else {
            return hours + ':' + minutes + ':' + seconds;
        }
        
    };

    return sessionTimeout;
}(sessionTimeout || {}));
