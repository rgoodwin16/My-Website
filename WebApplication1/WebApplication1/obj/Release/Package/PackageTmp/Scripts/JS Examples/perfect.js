//Perfect Numbers

function perfectChecker(n) {
    var temp = 0;
    for (var i = 1; i <= n / 2; i++) {
        if (n % i === 0) {
            temp += i;
        }
    }
    if (temp === n) {
        return true;
       
    } else {
        return false;
        
    }
}

function displayPerfect(num) {
    var displayHtml = document.getElementById('resultPerfect');

    for (j = num; j < 10000; j++) {
        if (perfectChecker(j)) {
            displayHtml.innerHTML += j + " is a Perfect Number" + "<br/>";
        }
    }
}
