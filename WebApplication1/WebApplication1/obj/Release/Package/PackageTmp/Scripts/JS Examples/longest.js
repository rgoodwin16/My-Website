//Longest Word 

(function () {
   
    //============= Longest Word Reader =====================================//

    window.longestReadText = function () {
        var reader = new FileReader();
        
        var output = "";

        var filePath = document.getElementById('fileInput');

        if (filePath.files && filePath.files[0]) {
            reader.onload = function (e) {
                output = e.target.result;
                var longest = longestWordChecker(output);
                displayLongestWord(longest)
            }; //end onload()
            reader.readAsText(filePath.files[0]);
        } //end if html5 filelist support
    }

    //============= Longest Word Checker & Display =========================//

    function longestWordChecker(txt) {
        var str = txt.split(new RegExp(/[\s,()\n\r\t]+/));
        
        console.log(str);
        var longest = ' ';
        for (var i = 0; i < str.length; i++) {
            
            var current = str[i];
            if (current.length > longest.length)
                longest = current;
        }
        return longest;
    }

    
    function displayLongestWord(word) {

        var longestResult = document.getElementById('resultLongest');
        longestResult.innerHTML = "The longest word in this file is: " + "<strong>" + word + "</strong>";

    }

})();