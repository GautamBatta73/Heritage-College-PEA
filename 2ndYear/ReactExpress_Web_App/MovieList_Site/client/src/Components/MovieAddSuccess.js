import React, { useState, useEffect } from 'react';

export default function MovieAddSuccess(props) {
    const [open, setOpen] = useState(true);

    useEffect(() => {
        setOpen(true);
    }, [props.title]);

    return (
        <div className={`modal ${open ? 'open' : 'closed'}`}>
            <div className="modal-content">
                <h2>Movie Added Successfully!</h2>
                <p>{props.title} was added to your list.</p>
                <button onClick={() => setOpen(false)}>
                    Close
                </button>
            </div>
        </div>
    );
}