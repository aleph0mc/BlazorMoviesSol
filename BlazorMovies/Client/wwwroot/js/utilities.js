function my_function(message) {
    console.log('from utilities', message);
}

function dotnetStaticInvocation() {
    DotNet.invokeMethodAsync("BlazorMovies.Client", "GetCurrentCount")
        .then(result => {
            console.log("count from javascript", result);
        });
}

function dotnetInstanceInvocation(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount"); //no ".then" function for the method return a void
}

function initializeInactivityTimer(dotnetHelper) {
    var timer;
    document.onmousemove = document.onkeypress = () => {
        console.log('JS initializeInactivityTimer OK');
        clearTimeout(timer);
        timer = setTimeout(logout, 1000 * 60 * 5); // 5 minutes
    }

    function logout() {
        console.log('JS initializeInactivityTimer logout...');
        dotnetHelper.invokeMethodAsync("Logout");
    }
}

function setInLocalStorage(key, value) {
    localStorage[key] = value;
}

function getFromLocalStorage(key) {
    return localStorage[key];
}

//Another version
//function initializeInactivityTimer2(dotnetHelper) {
//    var timer;
//    document.onmousemove = resetTimer;
//    document.onkeypress = resetTimer;

//    function resetTimer() {
//        clearTimeout(timer);
//        timer = setTimeout(logout, 3000);
//    }

//    function logout() {
//        dotnetHelper.invokeMethodAsync("Logout");
//    }
//}
