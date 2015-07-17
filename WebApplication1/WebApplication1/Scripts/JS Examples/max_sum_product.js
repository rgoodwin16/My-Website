// Coder Foundry Javascript Examples | Ray Goodwin

//Max Of Five | Sum of Five | Product of Five

function maxOfFive() {
    var num1 = document.getElementById('num1').value;
    var num2 = document.getElementById('num2').value;
    var num3 = document.getElementById('num3').value;
    var num4 = document.getElementById('num4').value;
    var num5 = document.getElementById('num5').value;

    var max = Math.max(num1, num2, num3, num4, num5);

    var resultMax = document.getElementById('resultMax');
    resultMax.innerHTML = "The largest number is: " + max;

}

function sum(previousValue, currentValue) {
    return previousValue + Math.round(currentValue);
}

function multiply(previousValue, currentValue) {
    return previousValue * Math.round(currentValue);
}

function sumOfFive() {
    var numbers = [
        document.getElementById('num1').value,
        document.getElementById('num2').value,
        document.getElementById('num3').value,
        document.getElementById('num4').value,
        document.getElementById('num5').value,
    ];

    var sumResult = numbers.reduce(sum, 0);
    var sumHtml = document.getElementById('resultSum');
    sumHtml.innerHTML = "The Sum of the numbers is: " + sumResult;

}

function multiplyFive() {
    var numbers = [
        document.getElementById('num1').value,
        document.getElementById('num2').value,
        document.getElementById('num3').value,
        document.getElementById('num4').value,
        document.getElementById('num5').value,
    ];

    var multiplyResult = numbers.reduce(multiply, 1);
    var multiplyHtml = document.getElementById('resultMultiply');
    multiplyHtml.innerHTML = "The Product of the numbers is: " + multiplyResult;

}
