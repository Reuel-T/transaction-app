<template>
  <form @submit.prevent="onSubmit">
    <!-- Comment -->
    <input
      v-model.trim="formData.comment"
      type="text"
      placeholder="Comment"
      :disabled="isPosting"
    />
    <!-- Amount -->
    <input
      v-model.number="formData.amount"
      type="number"
      placeholder="Amount"
      :disabled="isPosting"
    />
    <!-- Transaction Type -->
    <select
      v-model.number="formData.transactionTypeID"
      :disabled="isPosting"
    >
      <option value="1">Debit</option>
      <option value="2">Credit</option>
    </select>
    <button type="submit">Submit</button>
  </form>
  {{ formData }}
</template>

<script setup lang="ts">
  import { reactive } from 'vue'
  import type { CreateClientTransactionDTO } from '@/models/CreateClientTransactionDTO'
  import { usePostClientTransaction } from '@/shared/usePostClientTransaction'

  //props taken in, just the client ID
  const props = defineProps({
    clientId: {
      required: true,
      type: Number
    }
  })

  //form data object with default values assigned
  const formData = reactive<CreateClientTransactionDTO>({
    clientID: props.clientId,
    amount: 0,
    comment: '',
    transactionTypeID: 1
  })

  const emit = defineEmits(['post-success'])

  //is posting - ref value used to disable fields during request
  //post data - function to call when ready to post data
  const { isPosting, postData } = usePostClientTransaction({
    onSuccess: () => {
      emit('post-success', { ...formData })
      //reset the form
      formData.amount = 0
      formData.comment = ''
      formData.transactionTypeID = 1
    }
  })
  //post the form data to the API
  function onSubmit() {
    postData(formData)
  }
</script>

<style lang="scss" scoped></style>
