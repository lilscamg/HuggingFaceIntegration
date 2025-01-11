import { TaskModel } from "../models";

export const textClassificationModels: Array<TaskModel> = [
    {
        model: "distilbert-base-uncased-finetuned-sst-2-english",
        name: "Sentiment Analysis"
    },
    {
        model: "papluca/xlm-roberta-base-language-detection",
        name: "Language Detection"
    },
    {
        model: "s-nlp/roberta_toxicity_classifier",
        name: "Toxicity Classifier"
    }
]

export const textSummarizeModels: Array<TaskModel> = [
    {
        model: "Falconsai/text_summarization",
        name: "Falconsai text summarization"
    },
    {
        model: "facebook/bart-large-cnn",
        name: "Facebook bart-large-cnn"
    },
    {
        model: "google/pegasus-xsum",
        name: "Google pegasus xsum"
    }
]
