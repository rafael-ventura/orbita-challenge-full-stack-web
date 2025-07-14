import type {Student} from "~/models/Student";

export function useStudentApi() {
    const config = useRuntimeConfig();
    const apiBaseUrl = config.public.apiBaseUrl;

    const fetchStudents = async (): Promise<Student[]> => {
        try {
            const response = await $fetch<Student[]>(`${apiBaseUrl}/students`, {method: "GET"});

            console.log("Resposta da API:", response);
            return response;
        } catch (err) {
            console.error(" Erro ao buscar alunos:", err);
            return [];
        }
    };


    const createStudent = async (student: Student) => {
        try {
            const payload = {
                name: student.name,
                email: student.email,
                ra: student.ra,
                cpf: student.cpf,
            };
            return await $fetch(`${apiBaseUrl}/students`, {
                method: "POST",
                body: payload,
            });
        } catch (err) {
            console.error(" Erro ao criar aluno:", err);
            throw err;
        }
    };

    const updateStudent = async (id: string, student: Partial<Student>) => {
        try {
            return await $fetch(`${apiBaseUrl}/students/${id}`, {
                method: "PUT",
                body: student,
            });
        } catch (err) {
            console.error(" Erro ao atualizar aluno:", err);
            throw err;
        }
    };

    const deleteStudent = async (id: string) => {
        try {
            return await $fetch(`${apiBaseUrl}/students/${id}`, {method: "DELETE"});
        } catch (err) {
            console.error(" Erro ao excluir aluno:", err);
            throw err;
        }
    };

    return {
        fetchStudents,
        createStudent,
        updateStudent,
        deleteStudent,
    };
}
