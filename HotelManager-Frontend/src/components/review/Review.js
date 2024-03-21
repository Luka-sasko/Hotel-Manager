import { convertRating } from "../../common/HelperFunctions";

const Review = ({ review }) => {
  const { id, rating, comment, userFullName, dateCreated } = review;

  const formattedDate = new Date(dateCreated).toLocaleDateString();

  return (
    <div className="review" key={id}>
      <div className="author-rating-section">
        <div className="review-author">{userFullName} :</div>
        <div className="review-rating">{convertRating(rating)}</div>
        <div className="review-date">{formattedDate}</div>
      </div>
      <div className="review-text">{comment}</div>
    </div>
  );
};

export default Review;
