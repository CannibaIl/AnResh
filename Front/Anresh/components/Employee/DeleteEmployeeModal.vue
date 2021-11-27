<template>
  <section>
    <b-modal
      :title="title"
      v-model="show"
      hide-footer
    >
     <footer class="d-flex justify-content-end">
            <b-button class="custom-btn mr-2" @click="show = false">Close</b-button>
            <b-button class="custom-btn" @click.prevent="deleteEmployees()" variant="danger">Delete</b-button>
        </footer>

    </b-modal>
  </section>
</template>

<script>

export default {
  data() { 
    return {
      employee: null,
      selectedEmployees: [],
      title: '',
      show: false,
    };
  },

  methods: {
    open(data) {
      if (Array.isArray(data)) {
        this.selectedEmployees = data;
        this.title = "Delete employees";
        this.employee = null;
      }
      else {
        this.employee = data;
        this.title = `Delete ${this.$getFullName(data)}?`;
      }
      this.show = true;
    },

    deleteEmployees() {  
      if(this.employee === null){
        const listId = this.selectedEmployees;
        this.$axios.delete("/api/employee/multiple", {data: listId})
            .then(() => {
              this.show = false;
              this.$emit('row-deleted', listId);
              this.$notifyWarn('DELETED', `${listId.length} employees`);
              })
            .catch(error => {
                this.$notifyError('ERROR',`${error}`);
            });
      } 
      else {
        this.$axios.delete("/api/employee/" + this.employee.id)
            .then(() => {
              this.show = false;
              this.$emit('row-deleted', this.employee);
              this.$notifyWarn('DELETED EMPLOYEE', this.$getFullName(this.employee));
              })
            .catch(error => {
                this.$notifyError('ERROR',`${error}`);
            });
      } 
    }

  }
};
</script>
