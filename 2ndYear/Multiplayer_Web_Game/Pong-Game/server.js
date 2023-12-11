const apiServer = require(`./api.js`);
const PORT = 3000;

const http = require(`http`);
const server = http.createServer(apiServer);
const sockets = require(`./socket.js`);

const io = require(`socket.io`)
const socketServer = io(server);

sockets(socketServer);

server.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});