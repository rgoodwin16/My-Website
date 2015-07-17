(function () {
    
    //============= Word Search Reader ======================================//

    window.wordsearchReadText = function () {
        var reader = new FileReader();
        
        var output = "";

        var filePath = document.getElementById('wordsearchFileInput');

        if (filePath.files && filePath.files[0]) {
            reader.onload = function (e) {
                output = e.target.result;
                var wordSearch = wordSearchChecker(output);
                displayWordSearch(wordSearch)
            }; //end onload()
            reader.readAsText(filePath.files[0]);
        } //end if html5 filelist support
    }
    
    //============= WordSearch Checker & Display ==========================//

    function wordSearchChecker(txt) {
        var userKeyWord = document.getElementById('userKeyWord').value;
        var wordSearch = '';
        var array = txt.match(new RegExp(userKeyWord, 'gi'));

        if (array == null) {
            
            wordSearch =  "<h4>" + userKeyWord + "</h4> " + " does not appear in this file.";

        } else if (array.length == 1) {
            wordSearch = "<h4>" + userKeyWord + "</h4> " + " appears " + array.length + " time.";
            wordSearch += "<br/>";
            wordSearch += "<br/>";
            wordSearch += txt.replace(new RegExp(userKeyWord, 'gi'), "<strong class='btn btn-warning'>" + userKeyWord + "</strong>");
        } else {
            wordSearch = "<h4>" + userKeyWord + "</h4> " + " appears " + array.length + " times.";
            wordSearch += "<br/>";
            wordSearch += "<br/>";
            wordSearch += txt.replace(new RegExp(userKeyWord, 'gi'), "<strong class='btn btn-warning'>" + userKeyWord + "</strong>");
        }

        return wordSearch;
    }

    function displayWordSearch(word) {
        var searchResult = document.getElementById('resultWordSearch');
        searchResult.innerHTML = word;
    }


})();
