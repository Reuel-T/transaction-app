<template>
  <!-- 
    Component that displays a single transaction 
    and update comment form
   -->
  <div class="transaction card">
    <div class="transaction-row">
      <!-- Transaction Amount -->
      <div class="transaction-row-item">
        <p
          class="money"
          :class="localTransaction.amount > 0 ? 'positive' : 'negative'"
        >
          {{ numberToCurrency(localTransaction.amount) }}
        </p>
      </div>
      <!-- Transaction Type -->
      <div class="transaction-row-item">
        <p>{{ localTransaction.transactionTypeName }}</p>
      </div>
      <!-- Comment -->
      <div class="transaction-row-item">
        <p>{{ localTransaction.comment }}</p>
      </div>
      <!-- Button to show/hide update comment form -->
      <div class="transaction-row-end">
        <button
          @click="
            () => {
              showForm = !showForm
            }
          "
          class="btn pill"
        >
          {{ showForm ? 'Close' : 'Edit' }}
        </button>
      </div>
    </div>

    <!-- Update comment form -->
    <form
      v-if="showForm"
      class="transaction-form"
      @submit.prevent="
        () => {
          handleSubmit()
        }
      "
    >
      <!-- Comment -->
      <div class="form-group">
        <label for="comment">Comment</label>
        <input
          id="comment"
          v-model="formData.comment"
          type="text"
          placeholder="Transaction Comment"
          :disabled="isUpdating"
        />
      </div>

      <!-- Submit Button -->
      <div class="form-group">
        <label for="update"><span>&ThickSpace;</span></label>
        <button
          class="btn pill"
          id="update"
          :type="'submit'"
          :disabled="isUpdating"
        >
          Update
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import { numberToCurrency } from '@/helpers/helpers'
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
  import { usePutTransactionComment } from '@/shared/usePutComment'
  import { ref, type PropType, watch, reactive } from 'vue'

  const showForm = ref<boolean>(false)

  const props = defineProps({
    transaction: {
      required: true,
      type: Object as PropType<ClientTransactionDTO>
    }
  })

  //set the transaction ID if update is needed
  const formData = reactive({
    transactionID: props.transaction.transactionID,
    comment: ''
  })

  /*
    create a transaction scoped locally to the component based on the one 
    passed in through props
  */
  const localTransaction = ref({ ...props.transaction })

  /* 
    watch for changes in the passed in prop.
    if the passed in prop changes, set the locally scoped
    transaction to the updated one
  */
  watch(props.transaction, (newValue) => {
    localTransaction.value = newValue
  })

  /* 
    When the post is successful, set the locally scoped
    transaction to the one passed in from the API
  */
  function commentUpdated(data: ClientTransactionDTO) {
    localTransaction.value.comment = data.comment
    formData.comment = ''
    showForm.value = false
  }

  /* 
    just a hook for managing these comment updates
  */
  const { isUpdating, putData } = usePutTransactionComment({ onSuccess: commentUpdated })

  /* 
    runs when the user submits the form
  */
  function handleSubmit() {
    putData(formData)
  }
</script>

<style lang="scss" scoped>
  .transaction {
    display: flex;
    flex-direction: column;
    margin-bottom: 0.5rem;

    .transaction-row {
      display: flex;
      flex-direction: row;
      padding: 0.0125rem 0;

      .transaction-row-item {
        display: flex;
        align-items: center;

        width: 30%;
        p {
          width: 100%;
          margin: 0;
          padding: 0;
          text-align: center;
        }

        p.money {
          text-align: right;
          margin-right: 1rem;
        }
      }

      .transaction-row-end {
        width: 10%;

        button.btn {
          width: 100%;
        }
      }
    }

    .transaction-form {
      display: flex;
      flex-direction: row;

      margin: 0.5rem;

      .form-group {
        display: flex;
        flex-direction: column;

        button.btn {
          width: 100%;
        }
      }

      .form-group:first-child {
        width: 90%;
        margin-right: 2rem;
      }
    }
  }
</style>
