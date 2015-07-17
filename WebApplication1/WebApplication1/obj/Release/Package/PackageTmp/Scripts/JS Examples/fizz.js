//FizzBuzz

function fizzBuzz() {
    var fizzHtml = document.getElementById('resultFizz');

    for (i = 1; i <= 100; i++) {
        if ((i % 3 == 0) && (i % 5 == 0)) {
            fizzHtml.innerHTML += "FizzBuzz" + " ";
        } else if ((i % 3 == 0)) {
            fizzHtml.innerHTML += "Fizz" + " ";
        } else if ((i % 5 == 0)) {
            fizzHtml.innerHTML += "Buzz" + " ";
        } else {
            fizzHtml.innerHTML += i + " ";
        }
    }
}