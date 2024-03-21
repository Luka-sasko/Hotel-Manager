import React from "react";
import ReviewAdd from "../components/review/ReviewAdd";
import { useParams } from "react-router-dom";
import { AddReviewNavbar } from "../components/navigation/AddReviewNavbar";

export const AddReviewPage = () => {
  const { roomId } = useParams();
  return (
    <div className="add-review-page page">
      <AddReviewNavbar />
      <div className="container">
        <ReviewAdd roomId={roomId} />
      </div>
    </div>
  );
};
