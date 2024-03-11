<template>
  <div class="card">
    <h2>Create New Transaction</h2>
    <form @submit.prevent="onSubmit">
      <!-- Comment -->
      <div class="form-group">
        <label for="comment">Comment</label>
        <input
          id="comment"
          v-model.trim="formData.comment"
          type="text"
          placeholder="Comment"
          :disabled="isPosting"
        />
      </div>

      <!-- Amount -->
      <div class="form-group">
        <label for="amount">Amount</label>
        <div class="transaction-amount-wrapper">
          <p>{{ transactionSymbol }}</p>
          <input
            required
            id="amount"
            v-model.number="formData.amount"
            type="number"
            :min="1"
            placeholder="Amount"
            :disabled="isPosting"
          />
        </div>
      </div>
      <!-- Transaction Type -->
      <div class="form-group">
        <label for="transactionType">Type</label>
        <div class="transaction-type-wrapper"></div>
        <select
          id="transactionType"
          v-model.number="formData.transactionTypeID"
          :disabled="isPosting"
        >
          <option value="1">Debit</option>
          <option value="2">Credit</option>
        </select>
      </div>
      <div class="form-group">
        <label
          for="submit"
          style="color: transparent"
          >Submit</label
        >
        <button
          id="submit"
          class="btn pill"
          type="submit"
          :disabled="isPosting"
        >
          Create
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue'
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

  const transactionSymbol = computed(() => {
    return formData.transactionTypeID === 1 ? '+' : '-'
  })

  const emit = defineEmits(['post-success'])

  //is posting - ref value used to disable fields during request
  //post data - function to call when ready to post data
  const { isPosting, postData } = usePostClientTransaction({
    onSuccess: (data) => {
      emit('post-success', { ...data })
      //reset the form
      formData.amount = 0
      formData.comment = ''
      formData.transactionTypeID = 1
    }
  })
  //post the form data to the API
  function onSubmit() {
    let transactionAmount = Math.abs(formData.amount)

    //if transaction is type credit, set amount to negative
    if (formData.transactionTypeID === 2) {
      transactionAmount = transactionAmount * -1
    }

    const dataToPost: CreateClientTransactionDTO = {
      amount: transactionAmount,
      clientID: formData.clientID,
      comment: formData.comment,
      transactionTypeID: formData.transactionTypeID
    }

    postData(dataToPost)
  }
</script>

<style lang="scss" scoped>
  .card {
    margin-bottom: 0.5rem;

    h2 {
      margin: 0;
      padding: 0.5rem;
    }
  }

  form {
    display: flex;
    width: 100%;

    .form-group {
      padding: 0.5rem;
      display: flex;
      flex-direction: column;
      flex-grow: 1;

      .transaction-amount-wrapper {
        display: flex;
        flex-direction: row;
        p {
          text-align: center;
          width: 1rem;
          font-weight: bold;
          align-self: center;
          justify-self: center;
          margin: 0;
        }
      }
    }
  }
</style>
