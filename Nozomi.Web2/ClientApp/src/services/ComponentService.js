

export default {
  getAll() {
    
  },
  
  getComponentValue(dataset, type) {
    if (dataset && dataset.length > 0) {
      let res = dataset.filter(c => c.type === type);

      if (res && res.length > 0) {
        return res[0].value;
      }
    }

    return null;
  }
}