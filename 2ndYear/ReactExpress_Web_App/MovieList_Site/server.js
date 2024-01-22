const PORT = 8888;
const path = require(`path`);
const fs = require(`fs`);
const express = require('express');
const app = express();
const movies = path.join(__dirname, "../data/movies.json");

app.use(express.static(path.join(__dirname, `../client/public`)));
app.use(express.json());

app.route("/movies/:id?").get((req, res) => {
    if (req.params.id == undefined) {
        fs.readFile(movies, (err, data) => {
            if (err) {
                res.header("Content-Type", 'text/plain');
                console.error(err);
                res.end(`An Unexpected Error Occured!\n${err}`)
            };
            let json = JSON.parse(data);

            json = json.map(mov => {
                return {
                    Key: mov.Key,
                    Title: mov.Title,
                    Year: mov.Year
                }
            });

            json.sort((a, b) => {
                let titleA = a.Title.toLowerCase();
                let titleB = b.Title.toLowerCase();

                if (titleA < titleB) {
                    return -1;
                }

                if (titleA > titleB) {
                    return 1;
                }

                return 0;
            });

            res.header("Content-Type", 'application/json');
            res.json(json);
        });
    } else if (isNaN(req.params.id)) {
        fs.readFile(movies, (err, data) => {
            if (err) {
                res.header("Content-Type", 'text/plain');
                console.error(err);
                res.end(`An Unexpected Error Occured!\n${err}`)
            }
            let json = JSON.parse(data);

            json.sort((a, b) => {
                let TitleA = a.Title.toLowerCase();
                let TitleB = b.Title.toLowerCase();

                if (TitleA < TitleB) {
                    return -1;
                }

                if (TitleA > TitleB) {
                    return 1;
                }

                return 0;
            });

            let searchId = req.params.id.toLowerCase();
            let result = json.filter(({ Title }) => Title.toLowerCase().includes(searchId));

            if (result !== undefined) {
                res.header("Content-Type", 'application/json');
                res.json(result);
            }
        });
    } else {
        fs.readFile(movies, (err, data) => {
            if (err) {
                res.header("Content-Type", 'text/plain');
                console.error(err);
                res.end(`An Unexpected Error Occured!\n${err}`)
            }
            let json = JSON.parse(data);

            json.sort((a, b) => {
                let TitleA = a.Title.toLowerCase();
                let TitleB = b.Title.toLowerCase();

                if (TitleA < TitleB) {
                    return -1;
                }

                if (TitleA > TitleB) {
                    return 1;
                }

                return 0;
            });

            let searchId = Number(req.params.id);
            let result = json.find(({ Key }) => Key === searchId);

            if (result !== undefined) {
                res.header("Content-Type", 'application/json');
                res.json(result);
            }
        });
    }
}).post((req, res) => {
    fs.readFile(movies, (err, data) => {
        if (err) {
            res.header("Content-Type", 'text/plain');
            console.error(err);
            res.end(`An Unexpected Error Occured!\n${err}`)
        }

        let moviesJSON = JSON.parse(data);
        let newMovie = req.body;
        let maxVal = 0;

        moviesJSON.map((el) => {
            let val = el.Key;
            maxVal = Math.max(maxVal, val);
        });

        newMovie.Key = maxVal + 1;
        moviesJSON.push(newMovie);
        fs.writeFile(movies, JSON.stringify(moviesJSON, null, 2), (err) => {
            if (err) {
                res.header("Content-Type", 'text/plain');
                console.error(err);
                res.end(`Movie Could Not Be Added!\n${err}`)
            } else {
                res.end('Movie added successfully!');
            }
        });
    });
});

app.route("/actors/:name").get((req, res) => {
    if (req.params.name == undefined) {
        res.header("Content-Type", 'application/json');
        res.json([{}]);
    } else {
        fs.readFile(movies, (err, data) => {
            if (err) {
                res.header("Content-Type", 'text/plain');
                console.error(err);
                res.end(`An Unexpected Error Occured!\n${err}`)
            }
            let json = JSON.parse(data);
            let searchId = req.params.name.toLowerCase();
            let result = json.filter(
                ({ Actors }) =>
                    Actors.some(actor => actor.toLowerCase().includes(searchId))
            );

            result = result.map(mov => {
                return {
                    Key: mov.Key,
                    Title: mov.Title,
                    Year: mov.Year
                }
            });

            result.sort((a, b) => {
                let TitleA = a.Title.toLowerCase();
                let TitleB = b.Title.toLowerCase();

                if (TitleA < TitleB) {
                    return -1;
                }

                if (TitleA > TitleB) {
                    return 1;
                }

                return 0;
            });

            if (result !== undefined) {
                res.header("Content-Type", 'application/json');
                res.json(result);
            }
        });
    }
});

app.route("/years/:year").get((req, res) => {
    if (isNaN(req.params.year)) {
        res.header("Content-Type", 'application/json');;
        res.json([{}]);
    } else {
        fs.readFile(movies, (err, data) => {
            if (err) {
                res.header("Content-Type", 'text/plain');
                console.error(err);
                res.end(`An Unexpected Error Occured!\n${err}`)
            }
            let json = JSON.parse(data);
            let searchId = Number(req.params.year);
            let result = json.filter(({ Year }) => Year === searchId);

            result = result.map(mov => {
                return {
                    Key: mov.Key,
                    Title: mov.Title
                }
            });

            result.sort((a, b) => {
                let TitleA = a.Title.toLowerCase();
                let TitleB = b.Title.toLowerCase();

                if (TitleA < TitleB) {
                    return -1;
                }

                if (TitleA > TitleB) {
                    return 1;
                }

                return 0;
            });

            if (result !== undefined) {
                res.header("Content-Type", 'application/json');
                res.json(result);
            }
        });
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});