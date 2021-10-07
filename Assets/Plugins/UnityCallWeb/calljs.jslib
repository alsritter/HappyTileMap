mergeInto(LibraryManager.library, {
  getMapData: function () {
    const map = localStorage.getItem('localMapData');
    var bufferSize = lengthBytesUTF8(map) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(map, buffer, bufferSize);
    return buffer;
  },

  ReportReady: function () {
    window.ReportReady();
  }
})
