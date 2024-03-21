import React, { useState } from "react";
import { useParams } from "react-router-dom";
import api_review from "../../services/api_review";

const ReviewAdd = () => {
  const { roomId } = useParams();
  const [rating, setRating] = useState(1);
  const [text, setText] = useState("");

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const review = { rating, Comment: text };

      await api_review.createReviewForRoom(roomId, review);

      setRating(1);
      setText("");
    } catch (error) {
      console.error("Error submitting review:", error);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="add-review-form edit-view-form">
      <h3 className="edit-view-header">Add Review</h3>
      <div className="rating">
        <label className="edit-view-label" htmlFor="rating">
          Rating:
        </label>
        <input
          type="number"
          id="rating"
          min={1}
          max={5}
          value={rating}
          onChange={(e) => setRating(e.target.value)}
          className="rating-input edit-view-import"
        />
      </div>
      <div className="review-text">
        <label className="edit-view-label" htmlFor="review-text">
          Review Text:
        </label>
        <textarea
          id="review-text"
          value={text}
          onChange={(e) => setText(e.target.value)}
          className="review-text-area edit-view-text-area"
        />
      </div>
      <button type="submit" className="submit-button edit-view-button">
        Submit Review
      </button>
    </form>
  );
};

export default ReviewAdd;
