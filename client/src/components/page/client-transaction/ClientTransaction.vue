<template>
  <div class="transaction">

    <div class="transaction-row">
      <div class="transaction-row-item">
        <p>{{ numberToCurrency(localTransaction.amount) }}</p>
      </div>
      <div class="transaction-row-item">
        <p>{{ localTransaction.transactionTypeName }}</p>
      </div>
      <div class="transaction-row-item">
        <p>{{ localTransaction.comment }}</p>
      </div>
      <div class="transaction-row-end">
        <button @click="() => {showForm = !showForm}" class="btn">Edit</button>
      </div>
    </div>

    
      <form v-if="showForm" class="transaction-form"
        @submit.prevent="
          () => {
            handleSubmit()
          }
        "
      >
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

        <div class="form-group">
          <label for="update">Update Comment</label>
          <button
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
  import { numberToCurrency } from '@/helpers/helpers';
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

    .transaction-row{
      display: flex;
      flex-direction: row;
      
      .transaction-row-item{
        width: 30%;
      }

      .transaction-row-end{
        width: 10%;
      }

    }

    .transaction-form{
      display: flex;
      flex-direction: row;

      .form-group{
        display: flex;
        flex-direction: column;
      }

    }
  }
</style>
