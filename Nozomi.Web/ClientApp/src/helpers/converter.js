export default {
  // Formats the array to the output shown below.
  // arrName[0]=1050&arrName[1]=2000
  arrayToString(arrName, arr) {
    let str = '';
    arr.forEach(function(element, index) {
      str += arrName + "[" + index + "]=" + element;
      if (index !== (arr.length - 1)) { // If the index is not beyond the array's size
        str += '&'
      }
    });
    return str;
  }
}
