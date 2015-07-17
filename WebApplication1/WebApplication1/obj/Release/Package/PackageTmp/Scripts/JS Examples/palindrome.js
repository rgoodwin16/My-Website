//Palindromes

function palindrome() {
    var text = document.getElementById('userText').value;
    var inverse = text.split("").reverse().join("");

    if (inverse == text) {
        var palResult = text + " is a Palindrome.";
    } else {
        var palResult = text + " is NOT a Palindrome.";
    }

    var palHtml = document.getElementById('resultPal');
    palHtml.innerHTML = palResult;
}