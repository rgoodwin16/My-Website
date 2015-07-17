
//Factorials


function fact(num) {
    if (num < 0)
        return 'Undefined';
    var fact = 1;
    for (var i = num; i > 1; i--)
        fact *= i;
    return fact;
}

function factorial() {
    var factNum = parseFloat(document.getElementById('factNum').value);
    var factHtml = document.getElementById('resultFact');

    if (isNaN(factNum)) {
        factHtml.innerHTML = "Please enter a real number.";
    } else {
        var factResult = fact(factNum);
        factHtml.innerHTML = "The Factorial of " + factNum + " is:  " + factResult;
    }

}
  

