<template>
  <div class="text-classifier">
    <!-- form  -->
    <form @submit.prevent="processText">
      <textarea v-model="inputText" placeholder="Enter your text here..."></textarea>
      <div class="flex-div">
        <select v-model="taskType">
          <option v-for="type in taskTypes" :value="type.value">{{ type.name }}</option>
        </select>
        <select v-model="selectedValue" >
          <option v-for="taskModel in taskModels" :value="taskModel.model">{{ taskModel.name }}</option>
        </select>
        <input type="file" @change="handleFileUpload" />
        <button type="submit">Get result</button>
      </div>
    </form>

    <!-- results -->
    <div v-if="result">
      <h2>Results:</h2>
      <div v-if="taskType == TaskType.TextClassification" v-for="resultItem in result">
        <p><strong>Label:</strong> {{ (resultItem as TextClassficationResponse).label }}</p>
        <p><strong>Score:</strong> {{ (resultItem as TextClassficationResponse).score }}</p>
      </div>
      <div v-if="taskType == TaskType.TextSummarization">
        <p><strong>Summary text:</strong> {{ (result[0] as TextSummarizationResponse).summaryText }}</p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive, ref, watch } from 'vue';
import axios from 'axios';
import { TextClassficationResponse, TextSummarizationResponse } from '../models';
import { TaskType } from '../enums';
import { getTokenFromCookie, textClassificationModels, textSummarizeModels, taskTypes } from '../utils';

export default defineComponent({
  name: 'HuggingFaceIntegrationView',
  setup() {
    const taskType = ref<TaskType>(TaskType.TextClassification);
    const taskModels = reactive(textClassificationModels);

    const inputText = ref<string>('');
    const selectedValue = ref<string>(taskModels[0].model);

    const result = ref<TextClassficationResponse[] | TextSummarizationResponse[]>();

    watch(taskType, (newValue, _) => {
      switch (newValue) {
        case TaskType.TextClassification:
          Object.assign(taskModels, textClassificationModels);
          break;
        case TaskType.TextSummarization:
          Object.assign(taskModels, textSummarizeModels);
          break;
      }

      result.value = null;
      selectedValue.value = null;
    })

    const processText = async () => {
      try {
        if (!inputText.value || !selectedValue.value) {
          throw Error("Input field or model name field are empty")
        }
        
        const response = await axios.post(
          process.env.VUE_APP_HUGGING_FACE_BACKEND_URL + "/api/get-result",
          { 
            inputs: inputText.value,
            modelName: selectedValue.value,
            taskType: taskType.value
          },
          {
            headers: {
              Authorization: `Bearer ${getTokenFromCookie()}`
            }
          }
        );
        
        result.value = response.data.data;
      } 
      catch (error) {
        const errorResponse = error?.response?.data?.error;

        if (!!errorResponse) {
          alert(errorResponse)
        } 
        else {
          alert(`Error classifying text: ${error}`)
        }
      }
    };

    const handleFileUpload = (event: Event) => {
      const target = event.target as HTMLInputElement;
      if (target.files && target.files.length > 0) {
        const file = target.files[0];
        const reader = new FileReader();
        reader.onload = (e) => {
          const content = e.target?.result as string;
          inputText.value = content;
        };
        reader.readAsText(file);
      }
    };

    return {
      taskModels,
      taskTypes,
      inputText,
      taskType,
      TaskType,
      selectedValue,
      result,
      processText,
      handleFileUpload
    };
  }
});
</script>

<style>
.text-classifier {
  padding: 0 20vw;
}

textarea {
  width: 100%;
  height: 200px;
}

.flex-div {
  display: flex;
  flex-direction: row;
  gap: 10px;
  padding: 10px 0;
}
</style>
