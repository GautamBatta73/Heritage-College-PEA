import { React, useState } from 'react';
import MovieAddSuccess from './MovieAddSuccess';

export default function MovieAdd() {
    const [success, setSuccess] = useState(false);
    const [successTitle, setSuccessTitle] = useState('');

    function addErr(msg, input, err) {
        let errSpan = document.querySelector("#errorSpan");
        errSpan.style.display = 'block';
        err.innerHTML += `<p>${msg}</p>`;
        input.classList.add(`errorInput`);
    }

    function remErrs(inputs, err) {
        let errSpan = document.querySelector("#errorSpan");
        errSpan.style.display = 'none';
        err.innerHTML = ``;
        inputs.forEach(e => e.classList.remove(`errorInput`));
    }

    async function formSubmit(e) {
        e.preventDefault();
        let err = document.querySelector("#errorDiv");
        let inputs = document.querySelectorAll("#newMovieForm input");
        remErrs(inputs, err);
        let valTitle = inputs[0].value.trim();
        let valGenre = inputs[1].value.trim();
        let valActors = inputs[2].value.trim();
        let valYear = inputs[3].value.trim();
        let valRuntime = inputs[4].value.trim();
        let valRevenue = inputs[5].value.trim();

        if (valTitle.length < 1)
            addErr(`Title is required.`, inputs[0], err);

        if (valGenre.length < 1)
            addErr(`Genre is required.`, inputs[1], err);

        if (valActors.length < 1)
            addErr(`Actors is required.`, inputs[2], err);

        if (valYear.length < 1)
            addErr(`Year is required.`, inputs[3], err);
        else if (isNaN(valYear))
            addErr(`Year must be a number.`, inputs[3], err);
        else if (Number(valYear) < 1890)
            addErr(`Year must be larger than 1890.`, inputs[3], err);

        if (valRuntime.length < 1)
            addErr(`Runtime is required.`, inputs[4], err);
        else if (isNaN(valRuntime))
            addErr(`Runtime must be a number.`, inputs[4], err);

        if (valRevenue.length < 1)
            addErr(`Revenue is required.`, inputs[5], err);
        else if (isNaN(valRuntime))
            addErr(`Revenue must be a number.`, inputs[5], err);

        if (!err.hasChildNodes()) {
            let obj = {
                "Key": -1,
                "Title": "",
                "Genre": [],
                "Actors": [],
                "Year": 0,
                "Runtime": 0,
                "Revenue": 0
            }

            obj.Title = valTitle;
            obj.Genre = valGenre.split(`,`);
            obj.Actors = valActors.split(`,`);
            obj.Year = Number(valYear);
            obj.Runtime = Number(valRuntime);
            obj.Revenue = Number(valRevenue);

            obj.Genre = obj.Genre.map(e => e.trim());
            obj.Actors = obj.Actors.map(e => e.trim());

            console.log(obj);

            let postMov = fetch(`/movies`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                mode: "same-origin",
                credentials: "same-origin",
                body: JSON.stringify(obj)
            }).then(response => {
                console.log('Response object: ', response);
                return response.text();
            }).then(text => {
                console.log('text object: ', text);
            }).catch(error => {
                addErr(`An Error Occured While Sending The Data:<br> ${error}`, document.querySelector(`#newMovieForm`), err);
            });
            console.log(postMov);
            setSuccessTitle(obj.Title);
            setSuccess(true);
            e.target.reset();
        } else
            err.scrollIntoView();
    }

    return (
        <div id='addMoviePage'>
            <h1>Add a Movie</h1>
            <form onSubmit={formSubmit} id='newMovieForm'>
                <label htmlFor="fldTitle">Title:</label>
                <input type="text" id="fldTitle" name="fldTitle" defaultValue="" />

                <label htmlFor="fldGenre">Genre:</label>
                <div>
                    <input type="text" id="fldGenre" name="fldGenre" defaultValue="" />
                    <br />
                    <span className='inputHint'>Seperate genres with comma.</span>
                </div>

                <label htmlFor="fldActors">Actor(s):</label>
                <div>
                    <input type="text" id="fldActors" name="fldActors" defaultValue="" />
                    <br />
                    <span className='inputHint'>Seperate actors with comma.</span>
                </div>

                <label htmlFor="fldYear">Year:</label>
                <input type="text" id="fldYear" name="fldYear" defaultValue="" />

                <label htmlFor="fldRuntime">Runtime(in Minutes):</label>
                <input type="text" id="fldRuntime" name="fldRuntime" defaultValue="" />

                <label htmlFor="fldRevenue">Revenue(in Millions):</label>
                <input type="text" id="fldRevenue" name="fldRevenue" defaultValue="" />

                <input type="submit" defaultValue="Submit" />
            </form>

            <span id='errorSpan' style={{ display: "none" }}>Errors:</span>
            <div id='errorDiv'></div>

            {success && <MovieAddSuccess title={successTitle} />}
        </div>

    )
}