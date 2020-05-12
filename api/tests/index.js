// very simple express server for serving static files
const express = require('express');
const app = express();

app.use(express.static('public', {
    etag: false
}));

app.listen(3000, () => console.log('listening on port 3000'));
