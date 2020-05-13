var TwitterPlugin = {

    //指定されたURLを開くJavascript
    OpenNewWindow:  function(openUrl){
        //引数の定義
        var url = Pointer_stringify(openUrl);

        //名前を指定して新しいウィンドウを開く
        window.open(url, "TweetWindow");
    }
};

mergeInto(LibraryManager.library, TwitterPlugin);
