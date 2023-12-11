const PORT = 3001;

const express = require(`express`);
const path = require(`path`);
const api = express();

api.use(express.static("public"));

api.route(`/`).get((req, res) => {
    res.setHeader(`Content-Type`, `text/html`);
    res.render(`index.html`);
});

// api.listen(PORT, () => {
//     console.log(`Server running on port ${PORT}`);
// });

module.exports = api;