<template>
  <section>
    <edit-department-modal
      ref="editDepartmentModal"
      @row-created="addDepartment"
      @row-updated="replaceDepartment"
    />
    <delete-department-modal
      ref="deleteDepartmentModal"
      :departments="departments"
      @row-deleted="handleDepartmentDeleted"
      @transfer-completed="handleTransferCompleted"
    />

    <b-button
      class="d-block mt-2 mb-4"
      variant="primary"
      v-if="!hasDepartments && !isBusy"
      @click.prevent="handleCreateDepartment()"
      ><fa icon="plus" style="transform: scale(0.8)" /> Add
    </b-button>

    <div
      v-if="isBusy || hasDepartments"
      class="big-table-wrap table-responsive"
    >
      <b-button
        @click.prevent="handleCreateDepartment()"
        class="mt-2"
        variant="primary"
        ><fa icon="plus" style="transform: scale(0.8)" /> Add</b-button
      >

      <b-table
        class="big-table big-table-pointer big-table-buttons"
        striped
        hover
        borderless
        :busy="isBusy"
        :items="departments"
        :fields="fields"
        thead-class="big-table-head"
        tbody-class="big-table-body"
        @row-clicked="goToEmployees"
      >
        <template #head(*)>
          <span></span>
        </template>

        <template #table-busy>
          <div class="text-center text-danger my-2">
            <b-spinner class="align-middle"></b-spinner>
            <strong>Loading...</strong>
          </div>
        </template>

        <template #cell(*)="row">
          <div class="d-flex table-btn-group">
            <button @click.prevent="handleEditDepartment(row.item)">
              <fa class="mr-2 table-ico table-ico-primary" icon="edit" />
            </button>
            <button @click.prevent="handleDeleteDepartment(row.item)">
              <fa class="table-ico table-ico-danger" icon="trash" />
            </button>
          </div>
        </template>
      </b-table>
    </div>
    <span v-else>There are no departments</span>
  </section>
</template>

<script>
import EditDepartmentModal from "~/components/Department/EditDepartmentModal.vue";
import DeleteDepartmentModal from "~/components/Department/DeleteDepartmentModal.vue";
export default {
  components: { EditDepartmentModal, DeleteDepartmentModal },

  data: () => ({
    isBusy: true,
    fields: [
      { key: "id", label: "Id", class: "col-auto" },
      { key: "name", label: "name", class: "col-5" },
      { key: "employeeCount", label: "Employees Count", class: "col-auto" },
      { key: "*", class: "col-auto" },
    ],
    departments: [],
  }),

  computed: {
    hasDepartments() {
      return this.departments.length > 0;
    },
  },

  async mounted() {
    let { data } = await this.$axios.get("/api/department");
    this.departments = data;
    this.isBusy = false;
  },

  methods: {
    goToEmployees(data, index) {
      this.$router.push({ name: "employees-id", params: { id: data.id } });
    },

    async handleTransferCompleted(department) {
      const index = this.departments.findIndex((x) => x.id === department.id);
      if (index !== -1) {
        this.departments.splice(index, 1, department);
      }
    },

    handleCreateDepartment() {
      this.$refs["editDepartmentModal"].open();
    },

    handleEditDepartment(department) {
      this.$refs["editDepartmentModal"].open(department);
    },

    handleDeleteDepartment(department) {
      this.$refs["deleteDepartmentModal"].open(department);
    },

    addDepartment(data) {
      this.departments.unshift(data);
    },

    replaceDepartment(data) {
      const index = this.departments.indexOf(data.oldDepartment);
      this.departments.splice(index, 1, data.newDepartment);
    },

    handleDepartmentDeleted(id) {
      this.departments = this.departments.filter((d) => d.id !== id);
    },
  },
};
</script>
