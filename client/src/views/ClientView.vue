<template>
  <AppPageWrapper>
    <div class="container">
      <!-- Back Button and title -->
      <div class="title-wrapper">
        <RouterLink
          to="/"
          role="button"
        >
          <button
            id="back"
            class="btn pill"
          >
            Back
          </button>
        </RouterLink>
        <h1 class="title">Client Transactions</h1>
      </div>

      <div
        class="container"
        v-if="foundClient && !isClientError"
      >
        <!-- Client Info -->
        <ClientInfoCard
          :client="client"
          :is-loading="isClientLoading"
        />

        <!-- Create Transaction Form -->
        <CreateClientTransactionForm
          @post-success="
            (data: ClientTransactionDTO) => {
              refetchClient()
              console.log(data)
              if (transactions) {
                transactions.push(data)
              }
            }
          "
          :client-id="Number($route.params.id)"
        />

        <!-- Transaction List -->
        <div
          class="container card"
          style="margin-bottom: 0.5rem"
        >
          <ClientTransactions
            v-if="
              !isTransactionsError && !isTransactionsLoading && !transactions404 && transactions
            "
            :transactions="transactions"
          />
        </div>
      </div>
    </div>
  </AppPageWrapper>
</template>

<script setup lang="ts">
  import AppPageWrapper from '@/components/global/AppPageWrapper.vue'
  import ClientInfoCard from '@/components/page/client-transaction/ClientInfoCard.vue'
  import ClientTransactions from '@/components/page/client-transaction/ClientTransactions.vue'
  import CreateClientTransactionForm from '@/components/page/client-transaction/CreateClientTransactionForm.vue'
  import { useGetClient } from '@/shared/useGetClient'
  import { useGetClientTransactions } from '@/shared/useGetClientTransactions'
  import { useRoute } from 'vue-router'
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'

  //gets client id from route
  const route = useRoute()

  //destructures hook for different fields
  const {
    client,
    foundClient,
    isError: isClientError,
    isLoading: isClientLoading,
    fetchClient: refetchClient
  } = useGetClient(Number(route.params.id))

  const {
    transactions,
    is404: transactions404,
    isLoading: isTransactionsLoading,
    isError: isTransactionsError
  } = useGetClientTransactions(Number(route.params.id))
</script>

<style lang="scss" scoped>
  .container {
    height: 100%;
    display: flex;
    flex-direction: column;
    overflow-y: hidden;
  }

  button#back {
    position: absolute;
    top: 0.5rem;
  }
</style>
