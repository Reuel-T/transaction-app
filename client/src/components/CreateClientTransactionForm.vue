<template>
  <form @submit.prevent="onSubmit">
    <input
      v-model.trim="formData.comment"
      type="text"
      placeholder="Comment"
      :disabled="isPosting"
    />
    <input
      v-model.number="formData.amount"
      type="number"
      placeholder="Amount"
      :disabled="isPosting"
    />
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

  const props = defineProps({
    clientId: {
      required: true,
      type: Number
    }
  })

  const formData = reactive<CreateClientTransactionDTO>({
    clientID: props.clientId,
    amount: 0,
    comment: '',
    transactionTypeID: 1
  })

  const emit = defineEmits(['post-success'])

  const { isPosting, postData } = usePostClientTransaction({
    onSuccess: () => {
      emit('post-success', { ...formData })
      formData.amount = 0
      formData.comment = ''
      formData.transactionTypeID = 1
    }
  })

  function onSubmit() {
    postData(formData)
  }
</script>

<style lang="scss" scoped></style>
